using Orders.API.Clients;
using Orders.API.Persistence;
using Orders.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    // La validación la hace el Service para poder devolver el errorCode ORD-002.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Clientes HTTP tipados hacia los otros servicios. Las URLs salen de appsettings.json.
builder.Services.AddHttpClient<IUsersClient, UsersClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:UsersApi"]
        ?? throw new InvalidOperationException("Falta la configuración Services:UsersApi."));
    client.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddHttpClient<IProductsClient, ProductsClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:ProductsApi"]
        ?? throw new InvalidOperationException("Falta la configuración Services:ProductsApi."));
    client.Timeout = TimeSpan.FromSeconds(5);
});

var app = builder.Build();

app.MapControllers();

app.Run();
