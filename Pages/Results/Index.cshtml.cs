
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentElectionSystem.Data;

namespace StudentElectionSystem.Pages.Results
{
    [Authorize(Roles = "Admin,Supervisor")]
    public class ResultsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ResultsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ResultRow> Results { get; set; } = new();

        public async Task OnGetAsync()
        {
            // 1️⃣ Get vote counts grouped by CandidateId
            var voteCounts = await _context.Votes
                .GroupBy(v => v.CandidateId)
                .Select(g => new
                {
                    CandidateId = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // 2️⃣ Get all candidates
            var candidates = await _context.Candidates
                .ToListAsync();

            // 3️⃣ Merge results in memory (safe for EF Core)
            Results = candidates
                .Select(c => new ResultRow
                {
                    Name = c.FullName,
                    ImagePath = c.ImagePath,
                    Votes = voteCounts
                        .FirstOrDefault(v => v.CandidateId == c.Id)?.Count ?? 0
                })
                .OrderByDescending(r => r.Votes)
                .ToList();
        }
    }

    public class ResultRow
    {
        public string Name { get; set; } = string.Empty;
        public int Votes { get; set; }
        public string? ImagePath { get; set; }
    }
}
