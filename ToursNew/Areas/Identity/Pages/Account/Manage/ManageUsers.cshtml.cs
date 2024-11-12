using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


[Authorize(Roles = "Admin")]
public class ManageUsersModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public ManageUsersModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        
    }

    [BindProperty]
    public List<IdentityUser> Users { get; set; }

    [BindProperty]
    public CreateUserInputModel NewUser { get; set; }

    [BindProperty]
    public UpdateUserInputModel EditUser { get; set; }
    
    public class CreateUserInputModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class UpdateUserInputModel
    {
        [Required]
        public string UserId { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }
    }

    public async Task OnGetAsync()
    {
        Users = _userManager.Users.ToList();
    }

    public async Task<IActionResult> OnPostAddUserAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var newUser = new IdentityUser { UserName = NewUser.Username, Email = NewUser.Email };
        var result = await _userManager.CreateAsync(newUser, NewUser.Password);
        if (result.Succeeded)
        {
            TempData["Message"] = "New user added successfully.";
        }
        else
        {
            TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateUserAsync()
    {
        var user = await _userManager.FindByIdAsync(EditUser.UserId);
        if (user == null) return NotFound();

        user.Email = EditUser.Email;
        user.UserName = EditUser.Username;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["Message"] = "User updated successfully.";
        }
        else
        {
            TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["Message"] = "User deleted successfully.";
        }
        else
        {
            TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return RedirectToPage();
    }
}
