using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services;
using BikeRentalApp.Services.Interfaces;
using BikeRentalApp.Messaging;
using BikeRentalApp.Messaging.Consumers;
using BikeRentalApp.Messaging.Publishers;
using BikeRentalApp.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<IBikeService, BikeService>();
builder.Services.AddScoped<IDeliveryPersonService, DeliveryPersonService>();
builder.Services.AddScoped<IRentalService, RentalService>();

// Database
builder.Services.AddDbContext<BikeRentalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Messaging
var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
builder.Services.AddSingleton(rabbitMQSettings);
builder.Services.AddSingleton<IPublisher, RabbitMQPublisher>();

builder.Services.AddHostedService<BikeCreatedConsumer>();

// Controllers (API)
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();