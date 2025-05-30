// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToursNew.Areas.Identity.Pages.Account.Manage;

public static class ManageNavPages
{
    public static string Index => "Index";

    public static string AdminPanel => "AdminPanel";

    public static string Email => "Email";

    public static string ChangePassword => "ChangePassword";

    public static string DownloadPersonalData => "DownloadPersonalData";

    public static string DeletePersonalData => "DeletePersonalData";

    public static string ExternalLogins => "ExternalLogins";

    public static string PersonalData => "PersonalData";

    public static string TwoFactorAuthentication => "TwoFactorAuthentication";

    public static string LicensePage => "LicensePage";

    public static string IndexNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, Index);
    }

    public static string EmailNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, Email);
    }

    public static string ChangePasswordNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, ChangePassword);
    }

    public static string DownloadPersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, DownloadPersonalData);
    }

    public static string DeletePersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, DeletePersonalData);
    }

    public static string ExternalLoginsNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, ExternalLogins);
    }

    public static string PersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, PersonalData);
    }

    public static string TwoFactorAuthenticationNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, TwoFactorAuthentication);
    }

    public static string AdminPanelNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, AdminPanel);
    }

    public static string LicensePageNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, LicensePage);
    }

    public static string PageNavClass(ViewContext viewContext, string page)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string
                         ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }
}