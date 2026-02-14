using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace StudentElectionSystemFull.Pages.Admin.Students
{
    public class UploadModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public UploadModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public IFormFile? StudentFile { get; set; }   // nullable because it may be empty

        public string Message { get; set; } = string.Empty;  // initialized

        public void OnGet()
        {
        }

        // 🔹 Upload File
        public async Task<IActionResult> OnPostAsync()
        {
            if (StudentFile == null || StudentFile.Length == 0)
            {
                Message = "Please select a valid file.";
                return Page();
            }

            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadPath);

            var fileName = Path.GetFileName(StudentFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await StudentFile.CopyToAsync(stream);
            }

            Message = "Student file uploaded successfully.";
            return Page();
        }

        // 🔹 Download Template (Matches Student Model Exactly)
        public IActionResult OnGetDownloadTemplate()
        {
            var csv = new StringBuilder();

            csv.AppendLine("StudentId,FullName,Faculty,Class,ImagePath,HasVoted");

            csv.AppendLine("ST001,Ahmed Ali,Engineering,CS-3,default.png,false");
            csv.AppendLine("ST002,Fatima Noor,Medicine,MD-2,default.png,false");

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());

            return File(bytes, "text/csv", "StudentRegistrationTemplate.csv");
        }
    }
}
