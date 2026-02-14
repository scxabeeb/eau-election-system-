using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentElectionSystem.Data;

namespace StudentElectionSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalStudents { get; private set; }
        public int TotalCandidates { get; private set; }
        public int TotalVotes { get; private set; }
        public string ElectionStatus { get; private set; } = string.Empty;

        public async Task OnGetAsync()
        {
            TotalStudents = await _context.Students.CountAsync();
            TotalCandidates = await _context.Candidates.CountAsync();
            TotalVotes = await _context.Votes.CountAsync();

            // Simple election status logic
            ElectionStatus = TotalVotes > 0 ? "Active" : "Not Started";
        }
    }
}
