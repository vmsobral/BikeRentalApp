namespace BikeRentalApp.Domain.Entities;

public class Bike
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
}