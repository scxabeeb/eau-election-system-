using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudentElectionSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}