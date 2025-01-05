using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
    
    public string LicenseState { get; private set; }
        
    public class AddUserInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
            
        [Required]
        [RegularExpression("^(?=.{6,32}$)(?=.*[A-Z])(?=.*[a-z])(?!.*(.)\\1).+$",
            ErrorMessage = "Hasło musi mieć conajmniej 6 znaków i nie mogą się powtarzać, posiadać conajmniej jedną dużą literę, oraz nie może być puste")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Hasła nie pasują do siebie")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "One time password")]
        public bool CheckboxOTP { get; set; }
        [Display(Name = "Set password expiration in seconds")]
        public int PasswordExpiration { get; set; }
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
        LicenseState = LicenseModel.GetLicenseState();
        Users = await _userManager.Users
            .Select(user => new IdentityUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
            })
            .ToListAsync();
        
        ActivityLogs = await _context.ActivityLogs
            .OrderByDescending(log => log.Timestamp)
            .ToListAsync();
    }

public async Task<IActionResult> OnPostAddUserAsync()
{
    if (!ModelState.IsValid)
    {
        _logger.LogError("Model state invalid");
    }

    string password = InputUserModel.CheckboxOTP ? GenerateOTP() : InputUserModel.Password;

    var user = new IdentityUser { UserName = InputUserModel.Email, Email = InputUserModel.Email };
    try
    {
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            if (InputUserModel.CheckboxOTP)
            {
                var otpClaim = new Claim("OTP", password); 
                await _userManager.AddClaimAsync(user, otpClaim);
                TempData["Message"] = $"New user added successfully. OTP generated: {password}";
            }
            else
            {
                TempData["Message"] = "New user added successfully.";
            }
            
            // Automatically confirm the user's email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

            if (!confirmResult.Succeeded)
            {
                _logger.LogError("Email confirmation failed: {Errors}",
                    string.Join(", ", confirmResult.Errors.Select(e => e.Description)));
                TempData["Error"] = "User created, but email confirmation failed.";
            }
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
        var useradmin = await _userManager.FindByEmailAsync(user.Email);

        if (user == null) return NotFound();
        if (useradmin.Email.Equals("admin@gmail.com"))
        {
            TempData["Error"] = "You cannot delete your administrator account.";
            return RedirectToPage();
        }

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
        _context.ActivityLogs.RemoveRange(_context.ActivityLogs);
        await _context.SaveChangesAsync(); 
        TempData["Message"] = "All logs have been cleared.";
        return RedirectToPage(); 
    }
    
    public async Task<IActionResult> OnPostUnlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToPage();
        }

        if (!user.LockoutEnabled)
        {
            TempData["Error"] = "User is not locked";
        }
        await _userManager.SetLockoutEndDateAsync(user, null); // Remove lockoust
        TempData["Message"] = $"User {user.Email} has been unlocked.";
        return RedirectToPage();
    }
    
    private string GenerateOTP()
    {
        Random random = new Random();
        var x = random.Next(1, 101);
        int a = 15;
        var result = a * Math.Sin(x);
        int formattedResult = Math.Abs((int)result);
        return formattedResult.ToString("D7");
    }
}