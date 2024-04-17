using System.ComponentModel.DataAnnotations.Schema;
using ToursNew.Models;

namespace ToursNew.ViewModels
{
    public class ReservationViewModel
    {
        public int IDReservation { get; set; }

        public int IDClient { get; set; }

        public int IDTrip { get; set; }

        public DateTime ReservationDate { get; set; }

        public PaymentMethod paymentMethod { get; set; }

        public PaymentStatus paymentStatus { get; set; }
    }
}
