using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace StudentElectionSystemFull.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _signInManager.PasswordSignInAsync(
                Input.Email,
                Input.Password,
                false,
                false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToPage("/Admin/Dashboard");

                    if (await _userManager.IsInRoleAsync(user, "Supervisor"))
                        return RedirectToPage("/Admin/Dashboard");

                    if (await _userManager.IsInRoleAsync(user, "Student"))
                        return RedirectToPage("/Vote/Verify");
                }

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
