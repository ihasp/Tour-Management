using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToursNew.Models;

namespace ToursNew.Data;

public class ToursContext : IdentityDbContext
{
    public ToursContext(DbContextOptions<ToursContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ActivityLogs> ActivityLogs { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Trip>().ToTable("Trip");
        modelBuilder.Entity<Client>().ToTable("Client");
        modelBuilder.Entity<Reservation>().ToTable("Reservation");
        modelBuilder.Entity<ActivityLogs>().ToTable("ActivityLogs");
    }
}