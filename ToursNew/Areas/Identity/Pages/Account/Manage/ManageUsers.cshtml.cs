using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Core.Types;
using ToursNew.Data;
using ToursNew.Models;

#nullable disable

namespace ToursNew.Areas.Identity.Pages.Account.Manage;

public class ManageUsersModel : PageModel
{
    private UserManager<IdentityUser> _userManager;
    private readonly ILogger<ManageUsersModel> _logger;
    private readonly ToursContext _context;
    public ManageUsersModel(UserManager<IdentityUser> userManager, ILogger<ManageUsersModel> logger, RoleManager<IdentityRole> roleManager, ToursContext context)
    {
        _userManager = userManager;
        _logger = logger;
        _context = context;
    }

    
    [BindProperty]
    public List<IdentityUser> Users { get; set; }

    [BindProperty]
    public AddUserInputModel InputUserModel { get; set; }

    [BindProperty]
    public UpdateUserInputModel EditUser { get; set; }
        
    public class AddUserInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
            
        [Required]
        [RegularExpression("^(?=.{6,32}$)(?=.*[A-Z])(?=.*[a-z])(?!.*(.)\\1).+$",
            ErrorMessage = "Hasło musi mieć conajmniej 6 znaków i nie mogą się powtarzać")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Hasła nie pasują do siebie")]
        public string ConfirmPassword { get; set; }
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
    
    [BindProperty]
    public List<ActivityLogs> ActivityLogs { get; set; }
    public async Task OnGetAsync()
    {
        Users = _userManager.Users.ToList();
        Users = _userManager.Users.ToList();
        ActivityLogs = await _context.ActivityLogs
            .OrderByDescending(log => log.Timestamp)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAddUserAsync()
    {
        _logger.LogInformation("Adding user");
        if (!ModelState.IsValid)
        {
            _logger.LogError("Model state invalid");
        }

        var user = new IdentityUser {UserName = InputUserModel.Email, Email = InputUserModel.Email};
        try
        {
            var result = await _userManager.CreateAsync(user, InputUserModel.Password);

            if (result.Succeeded)
            {
                TempData["Message"] = "New user added successfully.";
            }
            else
            {
                _logger.LogError("User creation failed: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during user creation: {Error}", ex.Message);
            TempData["Error"] = "An unexpected error occurred while creating the user.";
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
    
    public async Task<IActionResult> OnPostClearLogsAsync()
    {
        _context.ActivityLogs.RemoveRange(_context.ActivityLogs); // Clear all logs
        await _context.SaveChangesAsync(); // Save changes to the database
        TempData["Message"] = "All logs have been cleared.";
        return RedirectToPage(); // Refresh the page
    }
    
}