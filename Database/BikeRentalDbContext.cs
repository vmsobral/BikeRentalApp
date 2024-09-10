using Microsoft.EntityFrameworkCore;
using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Database;

public class BikeRentalDbContext : DbContext
{
    public BikeRentalDbContext(DbContextOptions<BikeRentalDbContext> options) : base(options) {}

    public DbSet<Bike> Bikes { get; set; }
    public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique constraints
        modelBuilder.Entity<Bike>()
            .HasIndex(b => b.LicensePlate)
            .IsUnique();

        modelBuilder.Entity<DeliveryPerson>()
            .HasIndex(dp => dp.Cnpj)
            .IsUnique();

        modelBuilder.Entity<DeliveryPerson>()
            .HasIndex(dp => dp.CnhNumber)
            .IsUnique();
    }
}