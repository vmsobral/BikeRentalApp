using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Services.Interfaces;

public interface IBikeService
{
    Task<Bike> CreateBikeAsync(Bike bike);
    Task<IEnumerable<Bike>> GetAllBikesAsync();
    Task<Bike> GetBikeByIdAsync(Guid bikeId);
    Task<Bike> GetBikeByLicensePlateAsync(string plate);
    Task UpdateLicensePlateAsync(string oldPlate, string newPlate);
    Task DeleteBikeAsync(Guid bikeId);
}