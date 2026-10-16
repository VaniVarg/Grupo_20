using Users.API.Persistence;
using Users.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    // La validación la hace el Service para poder devolver el errorCode USR-002.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Repositorio en memoria (temporal). Singleton para que los datos vivan
// mientras el servidor esté encendido.
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.MapControllers();

app.Run();
