using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentElectionSystem.Data;

namespace StudentElectionSystem.Pages.Vote
{
    public class VoteIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public VoteIndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int StudentId { get; set; }
        public List<StudentElectionSystem.Models.Candidate> Candidates { get; set; } = new();

        public void OnGet(int id)
        {
            StudentId = id;
            Candidates = _context.Candidates.ToList();
        }

        public IActionResult OnPost(int studentId, int candidateId)
        {
            var student = _context.Students.Find(studentId);

            if (student == null || student.HasVoted)
                return RedirectToPage("/Index");

            _context.Votes.Add(new StudentElectionSystem.Models.Vote
            {
                StudentId = studentId,
                CandidateId = candidateId
            });

            student.HasVoted = true;
            _context.SaveChanges();

            return RedirectToPage("/Vote/Success");
        }
    }
}
