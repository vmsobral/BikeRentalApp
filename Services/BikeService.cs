using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services.Interfaces;
using BikeRentalApp.Messaging.Publishers;

namespace BikeRentalApp.Services;

public class BikeService : IBikeService
{
    private readonly List<Bike> _bikes;
    private readonly List<Rental> _rentals;
    private readonly IPublisher _publisher;
    private readonly ILogger<BikeService> _logger;

    public BikeService(List<Bike> bikes, List<Rental> rentals, IPublisher publisher, ILogger logger)
    {
        _bikes = bikes;
        _rentals = rentals;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Bike> CreateBikeAsync(Bike bike)
    {
        if (_bikes.Any(b => b.Plate == bike.Plate))
        {
            throw new InvalidOperationException("A bike with this plate already exists.");
        }

        bike.Id = Guid.NewGuid();
        _bikes.Add(bike);

        var bikeCreatedEvent = new
        {
            bike.Id,
            bike.Year,
            bike.Model,
            bike.Plate
        };

        try
        {
            await _publisher.Publish(JsonSerializer.Serialize(bikeCreatedEvent));
        }
        catch
        {

        }

        return bike;
    }

    public IEnumerable<Bike> GetAllBikes()
    {
        return _bikes;
    }

    public Bike GetBikeById(Guid id)
    {
        return _bikes.FirstOrDefault(b => b.Id == id);
    }

    public Bike GetBikeByPlate(string plate)
    {
        return _bikes.FirstOrDefault(b => b.Plate == plate);
    }

    public void UpdatePlate(string oldPlate, string newPlate)
    {
        var bike = _bikes.FirstOrDefault(b => b.Plate == oldPlate);
        if (bike == null)
        {
            throw new ArgumentException("Bike not found.");
        }

        if (_bikes.Any(b => b.Plate == newPlate))
        {
            throw new InvalidOperationException("A bike with the new plate already exists.");
        }

        bike.Plate = newPlate;
    }

    public void DeleteBike(Guid bikeId)
    {
        var bike = _bikes.FirstOrDefault(b => b.Id == bikeId);
        if (bike == null)
        {
            throw new ArgumentException("Bike not found.");
        }

        var existingRental = _rentals.Any(r => r.BikeId == bikeId);
        if (existingRental)
        {
            throw new InvalidOperationException("Cannot delete bike with active or past rentals.");
        }

        _bikes.Remove(bike);
    }
}