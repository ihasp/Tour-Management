using System;
using System.Collections.Generic;

namespace Tours.Models
{
    public class Trip
    {
        public int ID { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public int ClientID { get; set; }
        public Client Client { get; set; }
    }
}
