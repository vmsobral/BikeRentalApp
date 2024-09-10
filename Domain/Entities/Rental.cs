using System;

namespace BikeRentalApp.Domain.Entities;

public class Rental
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public decimal TotalCost { get; set; }
    public bool IsReturned { get; set; }

    // Foreign key for Bike
    public Guid BikeId { get; set; }
    public Bike Bike { get; set; }

    // Foreign key for DeliveryPerson
    public Guid DeliveryPersonId { get; set; }
    public DeliveryPerson DeliveryPerson { get; set; }
}