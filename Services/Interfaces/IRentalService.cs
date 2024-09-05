using BikeRentalApp.Domain.Entities;
using System;

namespace BikeRentalApp.Services.Interfaces;

public interface IRentalService
{
    Rental CreateRental(Rental rental);
    Rental GetRentalById(Guid id);
    Rental ReturnBike(Guid rentalId, DateTime returnDate);
}