// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToursNew.Services;

namespace ToursNew.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IActivityLogger _activityLogger;
    private readonly HttpClient _httpClient;
    private readonly ILogger<LoginModel> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger,
        IActivityLogger activityLogger, UserManager<IdentityUser> userManager, HttpClient httpClient)
    {
        _signInManager = signInManager;
        _logger = logger;
        _activityLogger = activityLogger;
        _userManager = userManager;
        _httpClient = httpClient;
    }

    [BindProperty] public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData] public string ErrorMessage { get; set; }

    public async Task TriggerHoneyTokenAsync()
    {
        try
        {
            var canaryTokenURL = "";
            var response = await _httpClient.GetAsync(canaryTokenURL);
            if (response.IsSuccessStatusCode)
                await _activityLogger.LogAsync("HoneyToken", User.Identity.Name, "Honey token triggered");
            else
                await _activityLogger.LogAsync("HoneyToken", User.Identity.Name,
                    "Honey token triggered but response wasn't successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error during honey token trigger: {ex.Message}");
            TempData["Error"] = $"Error during honey token trigger {ex.Message}";
        }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (!Input.Captcha.Equals("7"))
        {
            ModelState.AddModelError(string.Empty, "Invalid value in captcha field");
            return Page();
        }

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }


            // Check if the user is locked out first
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                if (lockoutEnd.HasValue && lockoutEnd > DateTimeOffset.Now)
                {
                    var minutesRemaining = (lockoutEnd.Value - DateTimeOffset.Now).Minutes;
                    ModelState.AddModelError(string.Empty,
                        $"Your account is locked. Try again in {minutesRemaining} minutes.");

                    return Page();
                }

                await _userManager.SetLockoutEndDateAsync(user, null);
            }

            // Check for OTP if required
            var otpClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == "OTP");

            if (otpClaim != null && !string.IsNullOrEmpty(Input.OTP))
                if (Input.OTP != otpClaim.Value)
                {
                    ModelState.AddModelError(string.Empty, "Invalid OTP.");
                    return Page();
                }

            // Sign in with password and handle lockout on failure
            var signInResult = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password,
                Input.RememberMe, true);

            if (signInResult.Succeeded)
            {
                await _activityLogger.LogAsync("Login", User.Identity.Name, "user logged in.");
                if (Input.Email.Equals("canarytoken@gmail.com")) await TriggerHoneyTokenAsync();
                return LocalRedirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is locked. Please try again later.");
                _logger.LogWarning("User account locked out.");
                return Page();
            }

            if (signInResult.RequiresTwoFactor)
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });

            await _activityLogger.LogAsync("Failed Login", $"{Input.Email}", "Failed login attempt.");
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
        return Page();
    }
    
    public class InputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")] public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "S-Captcha")]
        [DataType(DataType.Text)]
        public string Captcha { get; set; }

        [Display(Name = "One-time password")]
        [DataType(DataType.Text)]

        public string OTP { get; set; }
    }
}