using Orders.API.Clients;
using Orders.API.ExceptionHandlers;
using Orders.API.Persistence;
using Orders.API.Services;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Formatting.Json;

// Serilog se configura ANTES que cualquier otra cosa, así se registra también el arranque.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    // Silenciamos el ruido interno del framework, pero dejamos ver el puerto en el que escucha.
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "Orders.API")
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [{Service}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/orders-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services
        .AddControllers()
        // La validación la hace el Service para poder devolver el errorCode del catálogo.
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

    // Manejo global de errores: los específicos primero, el genérico último.
    builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
    builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
    builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
    builder.Services.AddExceptionHandler<ConflictExceptionHandler>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // Inicio de cada request: agrega el endpoint a TODOS los logs del request.
    app.Use(async (context, next) =>
    {
        var endpoint = $"{context.Request.Method} {context.Request.Path}";

        using (LogContext.PushProperty("Endpoint", endpoint))
        {
            Log.Information("Inicio {Endpoint}", endpoint);
            await next();
        }
    });

    // Fin de cada request, con el status y la duración.
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "Fin {RequestMethod} {RequestPath} => {StatusCode} en {Elapsed:0.0} ms";
    });

    app.UseExceptionHandler();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Orders.API terminó de forma inesperada");
}
finally
{
    Log.CloseAndFlush();
}
