using Notifications.API.Clients;
using Notifications.API.ExceptionHandlers;
using Notifications.API.HealthChecks;
using Notifications.API.Middleware;
using Notifications.API.Persistence;
using Notifications.API.Services;
using Notifications.API.Swagger;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

// Serilog se configura ANTES que cualquier otra cosa, así se registra también el arranque.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "Notifications.API")
    // Consola legible. CorrelationId viene del scope que abre CorrelationIdMiddleware.
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [{Service}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    // Archivo JSON con todas las propiedades (Service, Endpoint, CorrelationId, ErrorCode...).
    .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/notifications-.log",
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

    builder.Services.AddSingleton<INotificationRepository, InMemoryNotificationRepository>();
    builder.Services.AddScoped<INotificationService, NotificationService>();

    // Correlation ID: permite leerlo desde los clientes HTTP y reenviarlo al otro servicio.
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddTransient<CorrelationIdDelegatingHandler>();

    // Cliente HTTP tipado hacia Users.API. La URL sale de appsettings.json.
    builder.Services.AddHttpClient<IUsersClient, UsersClient>(client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Services:UsersApi"]
            ?? throw new InvalidOperationException("Falta la configuración Services:UsersApi."));
        client.Timeout = TimeSpan.FromSeconds(5);
    })
    .AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

    // Cliente HTTP que usa solamente el health check, con un timeout más corto.
    builder.Services.AddHttpClient<UsersApiHealthCheck>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["Services:UsersApi"]!);
        client.Timeout = TimeSpan.FromSeconds(2);
    });

    // Health checks. Cada chequeo lleva una etiqueta que indica para qué endpoint cuenta:
    //   "live"  -> ¿el proceso está vivo?
    //   "ready" -> ¿está listo para atender (sus dependencias responden)?
    builder.Services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy("El servicio está en ejecución."), tags: ["live"])
        .AddCheck<UsersApiHealthCheck>("users-api", tags: ["ready"]);

    // Swagger: documentación interactiva en /swagger.
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Notifications API",
            Version = "v1",
            Description = "Registro y consulta de notificaciones. El envío es simulado."
        });

        // Toma los comentarios /// de controladores y DTOs (el .xml lo genera el compilador).
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Notifications.API.xml"));

        // Agrega a cada endpoint los ejemplos de error declarados con [ErrorResponse].
        options.OperationFilter<ErrorResponsesOperationFilter>();
    });

    // Manejo global de errores: los específicos primero, el genérico último.
    builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
    builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // Primer middleware: el Correlation ID tiene que existir para todo lo que sigue.
    app.UseMiddleware<CorrelationIdMiddleware>();

    // Inicio de cada request. Se usa app.Logger (ILogger) para que el log tome el CorrelationId del scope.
    app.Use(async (context, next) =>
    {
        var endpoint = $"{context.Request.Method} {context.Request.Path}";

        using (app.Logger.BeginScope(new Dictionary<string, object> { ["Endpoint"] = endpoint }))
        {
            app.Logger.LogInformation("Inicio {Endpoint}", endpoint);
            await next();
        }
    });

    // Fin de cada request, con status y duración. Este log lo escribe Serilog directamente,
    // así que el CorrelationId se le agrega de forma explícita.
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "Fin {RequestMethod} {RequestPath} => {StatusCode} en {Elapsed:0.0} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            if (httpContext.Items.TryGetValue(CorrelationIdMiddleware.ItemKey, out var id))
                diagnosticContext.Set("CorrelationId", id);
        };
    });

    app.UseExceptionHandler();

    // Swagger UI en /swagger.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapControllers();

    // GET /health       -> todos los chequeos
    // GET /health/live  -> solo los "live" (¿el proceso está vivo?)
    // GET /health/ready -> solo los "ready" (¿puede atender pedidos?)
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = HealthCheckResponseWriter.WriteAsync
    });
    app.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("live"),
        ResponseWriter = HealthCheckResponseWriter.WriteAsync
    });
    app.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready"),
        ResponseWriter = HealthCheckResponseWriter.WriteAsync
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Notifications.API terminó de forma inesperada");
}
finally
{
    Log.CloseAndFlush();
}
