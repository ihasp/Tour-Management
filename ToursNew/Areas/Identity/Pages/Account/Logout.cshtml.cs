// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToursNew.Services;

namespace ToursNew.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly IActivityLogger _activityLogger;
    private readonly ILogger<LogoutModel> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger,
        IActivityLogger activityLogger)
    {
        _signInManager = signInManager;
        _logger = logger;
        _activityLogger = activityLogger;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        await _activityLogger.LogAsync("Logout", User.Identity.Name, "User logged out.");
        if (returnUrl != null) return LocalRedirect(returnUrl);

        // This needs to be a redirect so that the browser performs a new
        // request and the identity for the user gets updated.
        return RedirectToPage();
    }
}