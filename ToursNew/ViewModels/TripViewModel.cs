using System.ComponentModel;

namespace ToursNew.ViewModels;

public class TripViewModel
{
    public int IDTrip { get; set; }

    [DisplayName("Cel wycieczki")] public string? Destination { get; set; }

    [DisplayName("Miejsce wylotu")] public string? FromWhere { get; set; }

    [DisplayName("Data wylotu")] public DateTime? DepartureDate { get; set; }

    [DisplayName("Data powrotu")] public DateTime? ReturnDate { get; set; }

    [DisplayName("Cena")] public decimal? Price { get; set; }

    [DisplayName("Opis")] public string? Description { get; set; }
}