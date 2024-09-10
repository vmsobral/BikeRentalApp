using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services.Interfaces;
using BikeRentalApp.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikeRentalApp.Services;

public class RentalService : IRentalService
{
    private readonly BikeRentalDbContext _context;

    public RentalService(BikeRentalDbContext context)
    {
        _context = context;
    }

    public async Task<Rental> CreateRentalAsync(Rental rental)
    {
        var bikeId = rental.BikeId;
        var bike = await _context.Bikes.FindAsync(bikeId);
        if (bike == null)
        {
            throw new ArgumentException($"No bike with id ${bikeId} found.");
        }

        var dpId = rental.DeliveryPersonId;
        var deliveryPerson = await _context.DeliveryPersons.FindAsync(dpId);
        if (deliveryPerson == null)
        {
            throw new ArgumentException($"No delivery person with id ${dpId} found.");
        }

        rental.Id = Guid.NewGuid();
        // This could be on frontend or different endpoint to show info to user
        rental.StartDate = DateTime.UtcNow.AddDays(1);
        rental.TotalCost = CalculateRentalCost(rental.ExpectedEndDate - rental.StartDate);

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return rental;
    }

    public async Task<Rental> GetRentalByIdAsync(Guid id)
    {
        return await _context.Rentals.FindAsync(id);
    }

    public async Task<Rental> ReturnBikeAsync(Guid rentalId, DateTime returnDate)
    {
        var rental = await _context.Rentals.FindAsync(rentalId);
        if (rental == null || rental.IsReturned)
        {
            throw new ArgumentException($"No active rental with id ${rentalId} found.");
        }

        rental.EndDate = returnDate;
        rental.IsReturned = true;

        if (returnDate < rental.ExpectedEndDate)
        {
            rental.TotalCost += CalculateEarlyReturnPenalty(rental.ExpectedEndDate - returnDate);
        }
        else if (returnDate > rental.ExpectedEndDate)
        {
            rental.TotalCost += CalculateLateReturnPenalty(returnDate - rental.ExpectedEndDate);
        }

        await _context.SaveChangesAsync();
        return rental;
    }

    private decimal CalculateRentalCost(TimeSpan rentalDuration)
    {
        int days = rentalDuration.Days;
        return days switch
        {
            <= 7 => days * 30m,
            <= 15 => days * 28m,
            <= 30 => days * 22m,
            <= 45 => days * 20m,
            _ => days * 18m,
        };
    }

    private decimal CalculateEarlyReturnPenalty(TimeSpan unusedDays)
    {
        int days = unusedDays.Days;
        return days switch
        {
            <= 7 => days * 30m * 0.20m,
            <= 15 => days * 28m * 0.40m,
            _ => 0m,
        };
    }

    private decimal CalculateLateReturnPenalty(TimeSpan additionalDays)
    {
        return additionalDays.Days * 50m;
    }
}