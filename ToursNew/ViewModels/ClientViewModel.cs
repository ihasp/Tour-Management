using System.ComponentModel;

namespace ToursNew.ViewModels;

public class ClientViewModel
{
    public int IDClient { get; set; }

    [DisplayName("Imię")] public string? Name { get; set; }

    [DisplayName("Nazwisko")] public string? LastName { get; set; }

    public string? Email { get; set; }

    [DisplayName("Nr telefonu")] public string? Phone { get; set; }

    [DisplayName("Dorosły")] public bool Adult { get; set; }
}