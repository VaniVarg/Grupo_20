using Notifications.API.Clients;
using Notifications.API.ExceptionHandlers;
using Notifications.API.Persistence;
using Notifications.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    // La validación la hace el Service para poder devolver el errorCode NTF-002.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddSingleton<INotificationRepository, InMemoryNotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Cliente HTTP tipado hacia Users.API. IHttpClientFactory administra las conexiones,
// y la URL sale de appsettings.json (así no queda escrita en el código).
builder.Services.AddHttpClient<IUsersClient, UsersClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:UsersApi"]
        ?? throw new InvalidOperationException("Falta la configuración Services:UsersApi."));
    client.Timeout = TimeSpan.FromSeconds(5);
});

// Manejo global de errores: los específicos primero, el genérico último.
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Activa los handlers. Tiene que ir ANTES de MapControllers.
app.UseExceptionHandler();

app.MapControllers();

app.Run();
