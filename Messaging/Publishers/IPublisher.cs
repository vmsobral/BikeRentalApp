using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Messaging.Publishers;

public interface IPublisher
{
    Task PublishBikeCreatedEvent(Bike bike);
}