namespace Orders.API.Exceptions;

/// <summary>Se lanza cuando se viola una regla de negocio, por ejemplo stock insuficiente (HTTP 422).</summary>
public class BusinessRuleException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}
