using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentElectionSystem.Data;

namespace StudentElectionSystem.Pages.Admin.Reports;

public class VotingReportModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public VotingReportModel(ApplicationDbContext context)
    {
        _context = context;
    }

    // Filters
    [BindProperty(SupportsGet = true)]
    public string? Faculty { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Class { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? CandidateId { get; set; }

    // Dropdowns
    public List<SelectListItem> Faculties { get; set; } = new();
    public List<SelectListItem> Classes { get; set; } = new();
    public List<SelectListItem> Candidates { get; set; } = new();

    // Summary cards
    public int TotalStudents { get; set; }
    public int TotalVoted { get; set; }
    public int NotVoted { get; set; }

    // Results
    public List<VotingResult> Results { get; set; } = new();

    public void OnGet()
    {
        Faculties = _context.Students
            .Select(s => s.Faculty)
            .Distinct()
            .Select(f => new SelectListItem { Text = f, Value = f })
            .ToList();

        Classes = _context.Students
            .Select(s => s.Class)
            .Distinct()
            .Select(c => new SelectListItem { Text = c, Value = c })
            .ToList();

        Candidates = _context.Candidates
            .Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.Id.ToString()
            })
            .ToList();

        var studentQuery = _context.Students.AsQueryable();

        if (!string.IsNullOrEmpty(Faculty))
            studentQuery = studentQuery.Where(s => s.Faculty == Faculty);

        if (!string.IsNullOrEmpty(Class))
            studentQuery = studentQuery.Where(s => s.Class == Class);

        TotalStudents = studentQuery.Count();

        var voteQuery =
            from v in _context.Votes
            join s in studentQuery on v.StudentId equals s.Id
            join c in _context.Candidates on v.CandidateId equals c.Id
            select new { s, c };

        if (CandidateId.HasValue)
            voteQuery = voteQuery.Where(x => x.c.Id == CandidateId);

        TotalVoted = voteQuery
            .Select(x => x.s.Id)
            .Distinct()
            .Count();

        NotVoted = TotalStudents - TotalVoted;

        Results = voteQuery
            .GroupBy(x => x.c.FullName)
            .Select(g => new VotingResult
            {
                CandidateName = g.Key,
                TotalVotes = g.Count()
            })
            .OrderByDescending(x => x.TotalVotes)
            .ToList();
    }

    public class VotingResult
    {
        public string CandidateName { get; set; } = string.Empty;
        public int TotalVotes { get; set; }
    }
}
