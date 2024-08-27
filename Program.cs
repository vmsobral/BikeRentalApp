using BikeRentalApp.Domain.Interfaces;
using BikeRentalApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddScoped<IBikeService, BikeService>();
builder.Services.AddScoped<IDeliveryPersonService, DeliveryPersonService>();

// Controllers (API)
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();