using System.ComponentModel.DataAnnotations;

namespace StudentElectionSystem.Models;

public class Candidate
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Faculty { get; set; } = string.Empty;

    [Required]
    public string Position { get; set; } = string.Empty;

    // Candidate photo filename
    public string ImagePath { get; set; } = "default.png";
}
