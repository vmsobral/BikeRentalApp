using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services;
using BikeRentalApp.Services.Interfaces;
using BikeRentalApp.Messaging;
using BikeRentalApp.Messaging.Consumers;
using BikeRentalApp.Messaging.Publishers;

var builder = WebApplication.CreateBuilder(args);

// Data Structures
var bikes = new List<Bike>();
builder.Services.AddSingleton(bikes);

var deliveryPersons = new List<DeliveryPerson>();
builder.Services.AddSingleton(deliveryPersons);

var rentals = new List<Rental>();
builder.Services.AddSingleton(rentals);

// Services
builder.Services.AddScoped<IBikeService, BikeService>();
builder.Services.AddScoped<IDeliveryPersonService, DeliveryPersonService>();
builder.Services.AddScoped<IRentalService, RentalService>();

// Database
builder.Services.AddDbContext<BikeRentalContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// RabbitMQ
var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
builder.Services.AddSingleton(rabbitMQSettings);
builder.Services.AddSingleton<IPublisher, RabbitMQPublisher>();

builder.Services.AddHostedService<BikeCreatedConsumer>();

// Controllers (API)
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();