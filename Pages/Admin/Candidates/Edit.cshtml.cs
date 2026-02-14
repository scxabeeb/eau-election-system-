using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Pages.Admin.Candidates
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Candidate Candidate { get; set; } = new();

        [BindProperty]
        public IFormFile? CandidateImage { get; set; }

        public IActionResult OnGet(int id)
        {
            var candidate = _context.Candidates.Find(id);

            if (candidate == null)
                return NotFound();

            Candidate = candidate;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var candidateInDb = _context.Candidates.Find(Candidate.Id);

            if (candidateInDb == null)
                return NotFound();

            candidateInDb.FullName = Candidate.FullName;
            candidateInDb.Faculty  = Candidate.Faculty;
            candidateInDb.Position = Candidate.Position;

            if (CandidateImage != null)
            {
                var folder = Path.Combine(_env.WebRootPath, "candidate-images");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(CandidateImage.FileName);
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await CandidateImage.CopyToAsync(stream);

                candidateInDb.ImagePath = fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
