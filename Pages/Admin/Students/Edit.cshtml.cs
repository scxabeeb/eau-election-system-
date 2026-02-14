using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;
using Microsoft.AspNetCore.Http;

namespace StudentElectionSystem.Pages.Admin.Students
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Student Student { get; set; } = new();

        // ✅ REQUIRED for image upload
        [BindProperty]
        public IFormFile? StudentImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // 📸 Handle image upload
            if (StudentImage != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "student-images");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(StudentImage.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await StudentImage.CopyToAsync(stream);
                }

                Student.ImagePath = fileName;
            }

            _context.Attach(Student).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Students/Index");
        }
    }
}
