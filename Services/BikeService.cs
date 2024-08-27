using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BikeRentalApp.Services;

public class BikeService : IBikeService
{
    static private readonly List<Bike> _bikes = new();

    public Bike AddBike(Bike bike)
    {
        bike.Id = Guid.NewGuid();
        _bikes.Add(bike);
        return bike;
    }

    public List<Bike> GetAllBikes()
    {
        return _bikes;
    }

    public Bike GetBikeById(Guid id)
    {
        return _bikes.FirstOrDefault(b => b.Id == id);
    }

    public Bike GetBikeByPlate(string plate)
    {
        return _bikes.FirstOrDefault(b => b.Plate == plate);
    }

    public Bike UpdatePlate(string updatedplate)
    {
        var bike = _bikes.FirstOrDefault(b => b.Plate == updatedplate);
        if (bike != null)
        {
            bike.Plate = updatedplate;
            return bike;
        }
        return null;
    }

    public bool DeleteBike(Guid id)
    {
        var bike = _bikes.FirstOrDefault(b => b.Id == id);
        if (bike != null)
        {
            _bikes.Remove(bike);
            return true;
        }
        return false;
    }
}