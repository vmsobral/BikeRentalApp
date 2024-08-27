using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Domain.Interfaces;

public interface IDeliveryPersonService
{
    DeliveryPerson AddDeliveryPerson(DeliveryPerson deliveryPerson);
    DeliveryPerson GetDeliveryPersonById(Guid id);
    DeliveryPerson GetDeliveryPersonByCnpj(string cnpj);
    DeliveryPerson UpdateDeliveryPerson(DeliveryPerson deliveryPerson);
    bool SaveCnhImage(Guid id, byte[] imageBytes, string fileName);
    bool DeleteDeliveryPerson(Guid id);
}