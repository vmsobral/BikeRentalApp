using System.Text;
using System.Threading.Tasks;

using RabbitMQ.Client;

using Newtonsoft.Json;

using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Messaging.Publishers;
public class RabbitMQPublisher : IPublisher, IDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQPublisher(RabbitMQSettings settings)
    {
        _settings = settings;

        var factory = new ConnectionFactory()
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(_settings.ExchangeName, ExchangeType.Direct);
    }

    public async Task PublishBikeCreatedEvent(Bike bike)
    {
        var message = JsonConvert.SerializeObject(bike);
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: _settings.ExchangeName,
                              routingKey: _settings.QueueName,
                              basicProperties: null,
                              body: body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}