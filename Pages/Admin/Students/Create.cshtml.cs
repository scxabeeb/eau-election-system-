using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Pages.Admin.Students;

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
    public Student Student { get; set; } = new();

    [BindProperty]
    public IFormFile? StudentImage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Handle image upload
        if (StudentImage != null && StudentImage.Length > 0)
        {
            var imageFolder = Path.Combine(_env.WebRootPath, "student-images");
            Directory.CreateDirectory(imageFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(StudentImage.FileName);
            var filePath = Path.Combine(imageFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await StudentImage.CopyToAsync(stream);

            Student.ImagePath = fileName;
        }

        _context.Students.Add(Student);
        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
