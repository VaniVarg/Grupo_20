using Users.API.Swagger;
using Microsoft.OpenApi.Models;
using Users.API.ExceptionHandlers;
using Users.API.Persistence;
using Users.API.Services;
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
    .Enrich.WithProperty("Service", "Users.API")
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [{Service}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/users-.log",
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

    builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
    builder.Services.AddScoped<IUserService, UserService>();

    // Manejo global de errores: los específicos primero, el genérico último.
    builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
    builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
    builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
    builder.Services.AddExceptionHandler<UnauthorizedExceptionHandler>();
    builder.Services.AddExceptionHandler<ForbiddenExceptionHandler>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    // Swagger: documentación interactiva en /swagger.
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Users API",
            Version = "v1",
            Description = "Registro, login y consulta de usuarios, con bloqueo de cuenta tras tres intentos fallidos."
        });

        // Toma los comentarios /// de controladores y DTOs (el .xml lo genera el compilador).
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Users.API.xml"));

        // Agrega a cada endpoint los ejemplos de error declarados con [ErrorResponse].
        options.OperationFilter<ErrorResponsesOperationFilter>();
    });

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

    // Swagger UI en /swagger.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Users.API terminó de forma inesperada");
}
finally
{
    Log.CloseAndFlush();
}
