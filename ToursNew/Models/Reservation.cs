using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToursNew.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int IDReservation { get; set; }

        [ForeignKey("Client")]
        public int IDClient { get; set; }

        [ForeignKey("Trip")]
        public int IDTrip { get; set;}

        public DateTime ReservationDate { get; set; }   

        public PaymentMethod paymentMethod {  get; set; }   

        public PaymentStatus paymentStatus { get; set; }    
    }
    public enum PaymentMethod
    {
        Pieniędzmi,
        Paypal,
        Przelew,
        Karta
    }

    public enum PaymentStatus
    {
        Oczekujący,
        Nieudany,
        Potwierdzony
    }
}
