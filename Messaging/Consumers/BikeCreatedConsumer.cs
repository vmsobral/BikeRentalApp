using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using BikeRentalApp.Services.Interfaces;
using BikeRentalApp.Domain.Entities;

namespace BikeRentalApp.Messaging.Consumers;

public class BikeCreatedConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IBikeService _bikeService;
    private readonly ILogger<BikeCreatedConsumer> _logger;
    private readonly string _queueName;

    public BikeCreatedConsumer(RabbitMQSettings settings, IBikeService bikeService, ILogger<BikeCreatedConsumer> logger)
    {
        _queueName = settings.QueueName;

        var factory = new ConnectionFactory()
        {
            HostName = settings.Host,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password,
            VirtualHost = settings.VirtualHost
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

        _bikeService = bikeService;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var bike = JsonSerializer.Deserialize<Bike>(message);

            if (bike.Year == 2024)
            {
                _logger.LogInformation("Save 2024 bike to database", bike);
            }

            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: _queueName,
                                autoAck: false,
                                consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}