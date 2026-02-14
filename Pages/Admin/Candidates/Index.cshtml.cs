using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Pages.Admin.Candidates;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public List<Candidate> Candidates { get; set; } = new();

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        Candidates = _context.Candidates.ToList();
    }
}
