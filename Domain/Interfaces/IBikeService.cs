using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Domain.Interfaces;

public interface IBikeService
{
    Bike AddBike(Bike bike);
    List<Bike> GetAllBikes();
    Bike GetBikeById(Guid id);
    Bike GetBikeByPlate(string plate);
    Bike UpdatePlate(string plate);
    bool DeleteBike(Guid id);
}