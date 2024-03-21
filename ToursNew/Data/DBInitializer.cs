using Azure.Identity;
using System.ComponentModel.DataAnnotations;
using Tours.Models;

namespace Tours.Data
{
    public static class DBInitializer
    {
        public static void Initialize(ToursContext context)
        {
            context.Database.EnsureCreated();

            if (context.Clients.Any())
            {
                return;
            }

            var clients = new Client[]
            {
                new Client{Name="John Doe", Email="john@example.com", Phone="555-1234"},
                new Client{Name="Alice Smith", Email="alice@example.com", Phone="555-5678"},
                new Client{Name="Bob Johnson", Email="bob@example.com", Phone="555-9012"}

            };

            foreach (Client c in clients)
            {
                context.Clients.Add(c);
            }

            context.SaveChanges();



            var trips = new Trip[]
            {
                new Trip {Destination="Puerto Rico", DepartureDate=DateTime.Parse("2024-06-22"), ReturnDate=DateTime.Parse("2024-08-22"),
                    Price=2000, Description="Vacation in Puerto Rico", ClientID=1},
                new Trip {Destination="Hawaii", DepartureDate=DateTime.Parse("2024-09-15"), ReturnDate=DateTime.Parse("2024-10-10"),
                    Price=3000, Description="Honeymoon in Hawaii", ClientID=2},
                new Trip {Destination="Bali", DepartureDate=DateTime.Parse("2024-07-10"),
                    ReturnDate=DateTime.Parse("2024-07-25"), Price=2500, Description="Adventure in Bali", ClientID=3}
            };

            foreach(Trip t in trips)
            {
                context.Trips.Add(t);
            }

            context.SaveChanges();  

        }
    }
}
