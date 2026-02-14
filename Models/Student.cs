using System.ComponentModel.DataAnnotations;

namespace StudentElectionSystem.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Faculty { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Class { get; set; } = string.Empty;

    // 📷 Registered student photo (uploaded by admin)
    public string ImagePath { get; set; } = "default.png";

    // 🗳 Voting status
    public bool HasVoted { get; set; }

    // 🔐 NEW: Verification layer
    public byte[]? VerificationPhoto { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
}
