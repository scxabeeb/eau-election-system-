using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Pages.Admin.Students;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty]
    public Student Student { get; set; } = new();

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnGet(int id)
    {
        Student = _context.Students.Find(id)!;
    }

    public IActionResult OnPost()
    {
        _context.Students.Remove(Student);
        _context.SaveChanges();
        return RedirectToPage("Index");
    }
}
