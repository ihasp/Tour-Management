using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using ToursNew.Models;

namespace ToursNew.ViewModels
{
    public class ReservationViewModel
    {
        public int IDReservation { get; set; }
        [DisplayName("ID Klienta")]
        public int ?IDClient { get; set; }
        [DisplayName("ID Wycieczki")]
        public int ?IDTrip { get; set; }
        [DisplayName("Data rezerwacji")]
        public DateTime ?ReservationDate { get; set; }
        [DisplayName("Metoda płatności")]
        public PaymentMethod paymentMethod { get; set; }
        [DisplayName("Status płatności")]
        public PaymentStatus paymentStatus { get; set; }
    }
}
