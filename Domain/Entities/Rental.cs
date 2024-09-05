using System;

namespace BikeRentalApp.Domain.Entities;

public class Rental
{
    public Guid Id { get; set; } // Unique identifier
    public Guid BikeId { get; set; } // Reference to the bike being rented
    public Guid DeliveryPersonId { get; set; } // Reference to the delivery person
    public DateTime StartDate { get; set; } // Date the rental starts
    public DateTime EndDate { get; set; } // Date the rental ends
    public DateTime ExpectedEndDate { get; set; } // Expected end date
    public decimal TotalCost { get; set; } // Total cost of the rental
    public bool IsReturned { get; set; } // Whether the bike has been returned
}