namespace BikeRentalApp.Messaging.Publishers;

public interface IPublisher
{
    Task Publish(string message);
}