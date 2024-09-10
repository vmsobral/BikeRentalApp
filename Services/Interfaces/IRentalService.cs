using BikeRentalApp.Domain.Entities;
using System;

namespace BikeRentalApp.Services.Interfaces;

public interface IRentalService
{
    Task<Rental> CreateRentalAsync(Rental rental);
    Task<Rental> GetRentalByIdAsync(Guid id);
    Task<Rental> ReturnBikeAsync(Guid rentalId, DateTime returnDate);
}