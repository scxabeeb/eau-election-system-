using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentElectionSystem.Data;

namespace StudentElectionSystem.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Summary stats
        public int TotalStudents { get; private set; }
        public int TotalCandidates { get; private set; }
        public int TotalVotes { get; private set; }

        // Chart data
        public List<string> CandidateNames { get; private set; } = new();
        public List<int> VoteCounts { get; private set; } = new();

        public async Task OnGetAsync()
        {
            // Accurate counts
            TotalStudents = await _context.Students.CountAsync();
            TotalCandidates = await _context.Candidates.CountAsync();
            TotalVotes = await _context.Votes.CountAsync();

            // Votes per candidate (ACCURATE)
            var results = await _context.Candidates
                .Select(c => new
                {
                    CandidateName = c.FullName,
                    Votes = _context.Votes.Count(v => v.CandidateId == c.Id)
                })
                .ToListAsync();

            CandidateNames = results.Select(r => r.CandidateName).ToList();
            VoteCounts = results.Select(r => r.Votes).ToList();
        }
    }
}
