using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;

namespace StudentElectionSystem.Pages.Vote;

public class VerifyModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public VerifyModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string StudentId { get; set; } = string.Empty;

    [BindProperty]
    public string CapturedImage { get; set; } = string.Empty;

    public string? Error { get; set; }

    public IActionResult OnPost()
    {
        // 🔒 Device lock
        if (HttpContext.Session.GetString("VERIFIED") == "YES")
        {
            Error = "Xogtan Horay Aya Lo Isticmalay. Mahadsanid.";
            return Page();
        }

        var student = _context.Students
            .FirstOrDefault(s => s.StudentId == StudentId);

        if (student == null)
        {
            Error = "Student not found.";
            return Page();
        }

        if (student.HasVoted)
        {
            Error = "You have already voted/ Codkaagi Wa Dhiibatay.";
            return Page();
        }

        if (string.IsNullOrEmpty(CapturedImage))
        {
            Error = "Please capture your photo.";
            return Page();
        }

        // 📸 Save webcam photo
        var base64 = CapturedImage.Split(',')[1];
        student.VerificationPhoto = Convert.FromBase64String(base64);
        student.IsVerified = true;
        student.VerifiedAt = DateTime.UtcNow;

        _context.SaveChanges();

        // 🔐 Lock browser
        HttpContext.Session.SetString("VERIFIED", "YES");

        return RedirectToPage("/Vote/Index", new { id = student.Id });
    }
}
