﻿using ToursNew.Models;

namespace ToursNew.Data;

public static class DBInitializer
{
    public static void Initialize(ToursContext context)
    {
        context.Database.EnsureCreated();

        if (context.Clients.Any() || context.Reservations.Any() || context.Trips.Any()) return;

        var clients = new Client[]
        {
            new()
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890",
                Adult = true
            },
            new()
            {
                Name = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Phone = "987-654-3210",
                Adult = true
            },
            new()
            {
                Name = "Alice",
                LastName = "Doe",
                Email = "alice.doe@example.com",
                Phone = "456-789-0123",
                Adult = false
            }
        };

        context.Clients.AddRange(clients);
        context.SaveChanges();

        var trips = new Trip[]
        {
            new()
            {
                Destination = "Paris",
                FromWhere = "New York",
                DepartureDate = DateTime.Parse("2024-05-01"),
                ReturnDate = DateTime.Parse("2024-05-10"),
                Price = 1500.00m,
                Description = "Explore the beautiful city of Paris."
            },

            new()
            {
                Destination = "Tokyo",
                FromWhere = "Los Angeles",
                DepartureDate = DateTime.Parse("2024-06-15"),
                ReturnDate = DateTime.Parse("2024-06-25"),
                Price = 2000.00m,
                Description = "Experience the vibrant culture of Tokyo."
            },
            new()
            {
                Destination = "Rome",
                FromWhere = "London",
                DepartureDate = DateTime.Parse("2024-07-20"),
                ReturnDate = DateTime.Parse("2024-07-30"),
                Price = 1800.00m,
                Description = "Discover the ancient history of Rome."
            }
        };

        context.Trips.AddRange(trips);
        context.SaveChanges();

        var reservations = new Reservation[]
        {
            new()
            {
                IDClient = clients.First().IDClient,
                IDTrip = trips.First().IDTrip,
                ReservationDate = DateTime.Now.AddDays(10),
                paymentMethod = PaymentMethod.Karta,
                paymentStatus = PaymentStatus.Potwierdzony
            },
            new()
            {
                IDClient = clients.Last().IDClient,
                IDTrip = trips.Last().IDTrip,
                ReservationDate = DateTime.Now.AddDays(15),
                paymentMethod = PaymentMethod.Paypal,
                paymentStatus = PaymentStatus.Oczekujący
            }
        };
        context.Reservations.AddRange(reservations);
        context.SaveChanges();
    }
}