using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Services.Interfaces;

public interface IDeliveryPersonService
{
    Task<DeliveryPerson> AddDeliveryPersonAsync(DeliveryPerson deliveryPerson);
    Task<DeliveryPerson> GetDeliveryPersonByIdAsync(Guid id);
    Task<DeliveryPerson> GetDeliveryPersonByCnpjAsync(string cnpj);
    Task UpdateDeliveryPersonAsync(DeliveryPerson deliveryPerson);
    Task SaveCnhImageAsync(Guid id, byte[] imageBytes, string fileName);
    Task DeleteDeliveryPersonAsync(Guid id);
}