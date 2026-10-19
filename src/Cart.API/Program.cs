using Cart.API.Clients;
using Cart.API.ExceptionHandlers;
using Cart.API.Persistence;
using Cart.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    // La validación de la cantidad la hace el Service para devolver CRT-004.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

// Cliente HTTP tipado hacia Products.API. IHttpClientFactory administra las conexiones,
// y la URL sale de appsettings.json (así no queda escrita en el código).
builder.Services.AddHttpClient<IProductsClient, ProductsClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:ProductsApi"]
        ?? throw new InvalidOperationException("Falta la configuración Services:ProductsApi."));
    client.Timeout = TimeSpan.FromSeconds(5);
});

// Manejo global de errores: los específicos primero, el genérico último.
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapControllers();

app.Run();