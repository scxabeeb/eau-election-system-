using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentElectionSystem.Models;

namespace StudentElectionSystem.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Vote> Votes => Set<Vote>();
}
