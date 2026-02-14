using System;

namespace StudentElectionSystem.Models;

public class Vote
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public int CandidateId { get; set; }

    public DateTime VotedAt { get; set; } = DateTime.Now;
}
