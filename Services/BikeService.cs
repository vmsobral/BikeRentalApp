using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using BikeRentalApp.Database;
using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services.Interfaces;
using BikeRentalApp.Messaging.Publishers;

namespace BikeRentalApp.Services;

public class BikeService : IBikeService
{
    private readonly BikeRentalDbContext _context;
    private readonly IPublisher _publisher;
    private readonly ILogger<BikeService> _logger;

    public BikeService(BikeRentalDbContext context, IPublisher publisher, ILogger<BikeService> logger)
    {
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Bike> CreateBikeAsync(Bike bike)
    {
        if (await _context.Bikes.AnyAsync(b => b.LicensePlate == bike.LicensePlate))
        {
            throw new InvalidOperationException("A bike with the same license plate already exists.");
        }

        bike.Id = Guid.NewGuid();
        _context.Bikes.Add(bike);
        await PublishBikeCreatedEvent(bike);
        await _context.SaveChangesAsync();

        return bike;
    }

    private async Task PublishBikeCreatedEvent(Bike bike) {
        try
        {
            await _publisher.PublishBikeCreatedEvent(bike);
        }
        catch (Exception ex)
        {
            _logger.LogError("Couldn't publish Bike Creation event", ex);
        }
    }

    public async Task<IEnumerable<Bike>> GetAllBikesAsync()
    {
        return await _context.Bikes.ToListAsync();
    }

    public async Task<Bike> GetBikeByIdAsync(Guid bikeId)
    {
        return await _context.Bikes.FindAsync(bikeId);
    }

    public async Task<Bike> GetBikeByLicensePlateAsync(string plate)
    {
        return await _context.Bikes
                .FirstOrDefaultAsync(b => b.LicensePlate == plate);
    }

    public async Task UpdateLicensePlateAsync(string oldPlate, string newPlate)
    {
        var bike = await _context.Bikes.FirstOrDefaultAsync(b => b.LicensePlate == oldPlate);
        if (bike == null)
        {
            throw new ArgumentException($"Bike with license plate ${oldPlate} not found.");
        }

        if (await _context.Bikes.AnyAsync(b => b.LicensePlate == newPlate))
        {
            throw new InvalidOperationException($"A bike with license plate ${newPlate} already exists.");
        }

        bike.LicensePlate = newPlate;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBikeAsync(Guid bikeId)
    {
        var bike = await _context.Bikes.FindAsync(bikeId);
        if (bike == null)
        {
            throw new ArgumentException("Bike not found.");
        }

        if (await _context.Rentals.AnyAsync(r => r.BikeId == bikeId))
        {
            throw new InvalidOperationException("Cannot delete bike with active or past rentals.");
        }

        _context.Bikes.Remove(bike);
        await _context.SaveChangesAsync();
    }
}