namespace Products.API.Exceptions
{
    // Se lanza cuando un recurso no existe (se traduce a HTTP 404)
    public class NotFoundException(string errorCode, string message) : Exception(message)
    {
        public string ErrorCode { get; } = errorCode;
    }
}
