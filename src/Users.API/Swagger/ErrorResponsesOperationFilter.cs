using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Users.API.Swagger;

/// <summary>
/// Lee los atributos [ErrorResponse] de cada endpoint y agrega a Swagger las respuestas de error
/// con ejemplos que siguen el contrato de la consigna (type, title, status, detail, instance,
/// errorCode, errorMessage y correlationId).
/// </summary>
public class ErrorResponsesOperationFilter : IOperationFilter
{
    private record ErrorInfo(string Type, string Title, string Detail, string Description);

    // Valores de Problem Details por status. Son los mismos que escriben los IExceptionHandler.
    private static readonly Dictionary<int, ErrorInfo> Infos = new()
    {
        [400] = new("https://tools.ietf.org/html/rfc7231#section-6.5.1", "Bad Request", "Los datos enviados son inválidos.", "Datos inválidos"),
        [401] = new("https://tools.ietf.org/html/rfc7235#section-3.1", "Unauthorized", "Las credenciales no son válidas.", "Credenciales incorrectas"),
        [403] = new("https://tools.ietf.org/html/rfc7231#section-6.5.3", "Forbidden", "El acceso está prohibido.", "Acceso denegado"),
        [404] = new("https://tools.ietf.org/html/rfc7231#section-6.5.4", "Not Found", "El recurso solicitado no fue encontrado.", "Recurso no encontrado"),
        [409] = new("https://tools.ietf.org/html/rfc7231#section-6.5.9", "Conflict", "Ya existe un recurso con esos datos.", "Conflicto con el estado actual del recurso"),
        [500] = new("https://tools.ietf.org/html/rfc7231#section-6.6.1", "Internal Server Error", "Ocurrió un error inesperado.", "Error interno del servidor"),
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var errors = context.MethodInfo.GetCustomAttributes<ErrorResponseAttribute>().ToList();
        var instance = "/" + context.ApiDescription.RelativePath;

        // Un status puede tener varios errorCode: se muestran como ejemplos seleccionables.
        foreach (var group in errors.GroupBy(e => e.StatusCode))
        {
            var info = Infos[group.Key];

            var examples = group.ToDictionary(
                e => e.ErrorCode,
                e => new OpenApiExample
                {
                    Summary = e.ErrorCode,
                    Value = BuildExample(group.Key, info, instance, e)
                });

            operation.Responses[group.Key.ToString()] = new OpenApiResponse
            {
                Description = info.Description,
                Content = { ["application/json"] = new OpenApiMediaType { Examples = examples } }
            };
        }
    }

    private static OpenApiObject BuildExample(int status, ErrorInfo info, string instance, ErrorResponseAttribute e) => new()
    {
        ["type"] = new OpenApiString(info.Type),
        ["title"] = new OpenApiString(info.Title),
        ["status"] = new OpenApiInteger(status),
        ["detail"] = new OpenApiString(info.Detail),
        ["instance"] = new OpenApiString(instance),
        ["errorCode"] = new OpenApiString(e.ErrorCode),
        ["errorMessage"] = new OpenApiString(e.ErrorMessage),
        ["correlationId"] = new OpenApiString("5f1c2d3e4a5b46789abc0123456789ab")
    };
}
