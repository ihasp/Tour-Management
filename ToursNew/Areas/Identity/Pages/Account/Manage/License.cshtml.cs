using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToursNew.Areas.Identity.Pages.Account.Manage;

public class LicenseModel : PageModel
{
    private const int CaesarShift = 3;

    private const string SecretKey = "TEST-SAMPLE-LICENSE";
    //WHVW-VDPSOH-OLFHQVH

    [BindProperty] public LicenseInputModel InputModel { get; set; }
    public string LicenseState { get; private set; }

    public static string GetLicenseState()
    {
        if (System.IO.File.Exists("license.txt"))
        {
            var licenseState = System.IO.File.ReadAllText("license.txt");
            return licenseState == "licensed" ? "Licensed" : "Demo-ware";
        }

        return "Demo-ware";
    }

    private static string EncryptCaesar(string input, int shift)
    {
        return new string(input.Select(c =>
            char.IsLetter(c)
                ? (char)((c - (char.IsUpper(c) ? 'A' : 'a') + shift) % 26 + (char.IsUpper(c) ? 'A' : 'a'))
                : c
        ).ToArray());
    }

    private static string DecryptCaesar(string input, int shift)
    {
        return EncryptCaesar(input, 26 - shift);
    }

    private bool IsValidLicenseKey(string licenseKey)
    {
        var decryptedKey = DecryptCaesar(licenseKey, CaesarShift);
        return decryptedKey == SecretKey;
    }

    private void SaveLicenseState(bool isLicensed)
    {
        var licenseState = isLicensed ? "licensed" : "demo";
        System.IO.File.WriteAllText("license.txt", licenseState);
    }

    public static bool LoadLicenseState()
    {
        if (System.IO.File.Exists("license.txt"))
            return System.IO.File.ReadAllText("license.txt") == "licensed";

        return false;
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please enter a valid license key.";
            return Page();
        }

        var generatedLicenseKey = EncryptCaesar(SecretKey, CaesarShift);

        //for debugging
        // string decryptedLicenseKey = DecryptCaesar(generatedLicenseKey, CaesarShift);
        // Console.WriteLine($"{generatedLicenseKey}: {decryptedLicenseKey}");
        // Console.WriteLine($"{InputModel.license}");

        if (generatedLicenseKey == InputModel.license)
            Console.WriteLine("Correct value for license key");
        else
            Console.WriteLine("Incorrect value for license key");

        if (IsValidLicenseKey(InputModel.license))
        {
            TempData["Message"] = "License activated! Full features unlocked.";
            SaveLicenseState(true);
        }
        else
        {
            TempData["Error"] = "Invalid license key. You are in demo mode.";
            SaveLicenseState(false);
        }

        LicenseState = GetLicenseState();
        return Page();
    }

    public async Task OnGetAsync()
    {
        LicenseState = GetLicenseState();
    }

    public class LicenseInputModel
    {
        [Required(ErrorMessage = "License field is empty")]
        [Display(Name = "License key")]
        public string license { get; set; }
    }
}