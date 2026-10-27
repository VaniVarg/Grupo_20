using Products.API.Clients;
using Products.API.ExceptionHandlers;
using Products.API.Persistence;
using Products.API.Services;
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
    .Enrich.FromLogContext()                          // incluye las propiedades que agregamos por request
    .Enrich.WithProperty("Service", "Products.API")   // en cada log va el nombre del servicio
                                                      // Consola: formato legible para humanos.
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [{Service}] {Message:lj}{NewLine}{Exception}")
    // Archivo: un JSON por evento, con TODAS las propiedades (Service, Endpoint, ErrorCode...).
    // Se crea un archivo por día en la carpeta logs/.
    .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/products-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Le decimos a ASP.NET que use Serilog: todos los ILogger<T> pasan a escribir por acá.
    builder.Host.UseSerilog();

    builder.Services
        .AddControllers()
        // Desactivamos el 400 automático de [ApiController]: la validación la hacemos
        // en la capa de servicios para poder devolver el errorCode PRD-002.
        .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

    builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();

    // Cliente HTTP tipado hacia Orders.API. La URL sale de appsettings.json.
    builder.Services.AddHttpClient<IOrdersClient, OrdersClient>(client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Services:OrdersApi"]
            ?? throw new InvalidOperationException("Falta la configuración Services:OrdersApi."));
        client.Timeout = TimeSpan.FromSeconds(5);
    });

    builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
    builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
    builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // Inicio de cada request: agrega el endpoint a TODOS los logs del request y registra el comienzo.
    app.Use(async (context, next) =>
    {
        var endpoint = $"{context.Request.Method} {context.Request.Path}";

        using (LogContext.PushProperty("Endpoint", endpoint))
        {
            Log.Information("Inicio {Endpoint}", endpoint);
            await next();
        }
    });

    // Fin de cada request, con el status y la duración. Va después del middleware anterior
    // para que este log también lleve la propiedad Endpoint.
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
    Log.Fatal(ex, "Products.API terminó de forma inesperada");
}
finally
{
    // Vacía el buffer para no perder los últimos logs al apagar el servicio.
    Log.CloseAndFlush();
}
