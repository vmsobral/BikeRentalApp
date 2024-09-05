using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikeRentalApp.Services;

public class RentalService : IRentalService
{
    private readonly List<Rental> _rentals = new();
    private readonly List<Bike> _bikes;
    private readonly List<DeliveryPerson> _deliveryPersons;

    public RentalService(List<Bike> bikes, List<DeliveryPerson> deliveryPersons)
    {
        _bikes = bikes;
        _deliveryPersons = deliveryPersons;
    }

    public Rental CreateRental(Rental rental)
    {
        var bike = _bikes.FirstOrDefault(b => b.Id == rental.BikeId);
        var deliveryPerson = _deliveryPersons.FirstOrDefault(dp => dp.Id == rental.DeliveryPersonId && dp.CnhType == CNHType.A);

        if (bike == null || deliveryPerson == null)
        {
            return null;
        }

        rental.Id = Guid.NewGuid();
        rental.StartDate = DateTime.UtcNow.AddDays(1);
        rental.TotalCost = CalculateRentalCost(rental.ExpectedEndDate - rental.StartDate);

        _rentals.Add(rental);
        return rental;
    }

    public Rental GetRentalById(Guid id)
    {
        return _rentals.FirstOrDefault(r => r.Id == id);
    }

    public Rental ReturnBike(Guid rentalId, DateTime returnDate)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental == null || rental.IsReturned)
        {
            return null;
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