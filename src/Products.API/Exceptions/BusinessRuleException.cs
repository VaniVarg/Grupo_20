namespace Products.API.Exceptions
{
    //Se lanza cuando se viola una regla de negocio (se traduce a HTTP 409)
    public class BusinessRuleException (string errorCode, string message) : Exception(message)
    {
        public string ErrorCode { get; } = errorCode;
    }
}
