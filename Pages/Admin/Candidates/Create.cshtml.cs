using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Pages.Admin.Candidates;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public CreateModel(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [BindProperty]
    public Candidate Candidate { get; set; } = new();

    [BindProperty]
    public IFormFile? CandidateImage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Handle image upload
        if (CandidateImage != null && CandidateImage.Length > 0)
        {
            var imageFolder = Path.Combine(_env.WebRootPath, "candidate-images");
            Directory.CreateDirectory(imageFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(CandidateImage.FileName);
            var filePath = Path.Combine(imageFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await CandidateImage.CopyToAsync(stream);

            Candidate.ImagePath = fileName;
        }

        _context.Candidates.Add(Candidate);
        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
