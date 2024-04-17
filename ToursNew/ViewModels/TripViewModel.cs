namespace ToursNew.ViewModels
{
    public class TripViewModel
    {
        public int IDTrip { get; set; }
        public string Destination { get; set; }
        public string FromWhere { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
