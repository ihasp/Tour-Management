﻿using Tours.Models;
using Microsoft.EntityFrameworkCore;


namespace Tours.Data
{
    public class ToursContext : DbContext
    {
        public ToursContext(DbContextOptions<ToursContext> options) : base(options)
        { 
        }
            public DbSet<Client> Clients { get; set; }
            public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>().ToTable("Trip");
            modelBuilder.Entity<Client>().ToTable("Client");
        }

    }
    }
