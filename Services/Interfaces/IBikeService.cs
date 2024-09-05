using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Services.Interfaces;

public interface IBikeService
{
    Task<Bike> CreateBikeAsync(Bike bike);
    IEnumerable<Bike> GetAllBikes();
    Bike GetBikeById(Guid id);
    Bike GetBikeByPlate(string plate);
    void UpdatePlate(string oldPlate, string newPlate);
    void DeleteBike(Guid id);
}