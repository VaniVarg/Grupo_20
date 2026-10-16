using Products.API.ExceptionHandlers;
using Products.API.Persistence;
using Products.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
  //Desactivamos el 400 automático de [ApiController]: la validación la hacemos en la capa de servicios para poder devolver el errorCode PRD-002.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Repositorio en memoria (temporal). Es Singleton para que los datos vivan mientras el servidor esté encendido.
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Manejo global de errores. El framework prueba los handlers EN ESTE ORDEN y usa el primero que devuelva true: los específicos van primero y el genérico, último.
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Activa los handlers.
app.UseExceptionHandler();

app.MapControllers();

app.Run();
