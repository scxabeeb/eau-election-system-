using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Pages.Admin.Students;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public List<Student> Students { get; set; } = new();

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        Students = _context.Students.ToList();
    }
}
