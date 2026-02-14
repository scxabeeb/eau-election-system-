using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Pages.Admin.Candidates;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty]
    public Candidate Candidate { get; set; } = new();

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnGet(int id)
    {
        Candidate = _context.Candidates.Find(id)!;
    }

    public IActionResult OnPost()
    {
        _context.Candidates.Remove(Candidate);
        _context.SaveChanges();
        return RedirectToPage("Index");
    }
}
