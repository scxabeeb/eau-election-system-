using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Pages.Admin.Reports;

public class StudentsListModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public StudentsListModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string Type { get; set; } = "all";

    [BindProperty(SupportsGet = true)]
    public string? Faculty { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Class { get; set; }

    public string PageTitle { get; set; } = string.Empty;

    public List<Student> Students { get; set; } = new();

    public async Task OnGetAsync()
    {
        var query = _context.Students.AsQueryable();

        // Filter by Faculty
        if (!string.IsNullOrEmpty(Faculty))
            query = query.Where(s => s.Faculty == Faculty);

        // Filter by Class
        if (!string.IsNullOrEmpty(Class))
            query = query.Where(s => s.Class == Class);

        // Filter by Voting Status
        if (Type == "voted")
        {
            PageTitle = "🗳️ Students Who Voted (Webcam Verified)";
            query = query.Where(s => s.HasVoted);
        }
        else if (Type == "notvoted")
        {
            PageTitle = "🚫 Students Who Have Not Voted";
            query = query.Where(s => !s.HasVoted);
        }
        else
        {
            PageTitle = "👨‍🎓 All Students";
        }

        Students = await query
            .OrderBy(s => s.FullName)
            .ToListAsync();
    }
}
