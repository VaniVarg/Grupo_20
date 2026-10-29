namespace Users.API.Swagger;

/// <summary>
/// Declara una respuesta de error de un endpoint (status, errorCode y errorMessage) para que
/// Swagger la muestre con su ejemplo. Se puede repetir: un endpoint puede devolver varios
/// errores con el mismo status, por ejemplo dos 404 distintos.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ErrorResponseAttribute(int statusCode, string errorCode, string errorMessage) : Attribute
{
    public int StatusCode { get; } = statusCode;
    public string ErrorCode { get; } = errorCode;
    public string ErrorMessage { get; } = errorMessage;
}
