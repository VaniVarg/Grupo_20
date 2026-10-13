using Cart.API.Persistence;
using Cart.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    // La validación de la cantidad la hace el Service para devolver CRT-004.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Repositorio en memoria (temporal). Singleton para que los datos vivan
// mientras el servidor esté encendido.
builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

var app = builder.Build();

app.MapControllers();

app.Run();
