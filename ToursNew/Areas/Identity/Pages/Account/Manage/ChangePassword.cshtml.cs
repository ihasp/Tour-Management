// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToursNew.Services;

namespace ToursNew.Areas.Identity.Pages.Account.Manage;

public class ChangePasswordModel : PageModel
{
    private readonly IActivityLogger _activityLogger;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChangePasswordModel> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public ChangePasswordModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<ChangePasswordModel> logger,
        IActivityLogger activityLogger,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _activityLogger = activityLogger;
        _configuration = configuration;
    }

    [BindProperty] public InputModel Input { get; set; }

    [TempData] public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        var hasPassword = await _userManager.HasPasswordAsync(user);
        if (!hasPassword) return RedirectToPage("./SetPassword");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        //recaptcha check
        var googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();
        var secretKey = _configuration["ReCaptchaSettings:SecretKey"];
        var verificationUrl = _configuration["ReCaptchaSettings:VerificationUrl"];

        var isValid = await ReCaptchaService.verifyReCaptchav3(googleRecaptchaToken, secretKey, verificationUrl);

        if (!isValid)
        {
            await _activityLogger.LogAsync("Password change", User.Identity.Name, "Invalid ReCaptchav3 token");
            TempData["Error"] = "Invalid Recaptcha";
            return RedirectToPage("./Index");
        }


        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        await _activityLogger.LogAsync("Password change", User.Identity.Name, "User changed their password");
        TempData["Message"] = "Your password has been changed.";
        return RedirectToPage();
    }

    public class InputModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression("^(?=.{6,32}$)(?=.*[A-Z])(?=.*[a-z])(?!.*(.)\\1).+$",
            ErrorMessage =
                "Hasło musi mieć conajmniej 6 znaków i nie mogą się powtarzać, posiadać conajmniej jedną dużą literę, oraz nie może być puste")]
        // [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}