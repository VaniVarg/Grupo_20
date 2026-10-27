namespace Notifications.API.ExceptionHandlers;

//Escribe la respuesta de error con el formato de la consigna: los campos estándar de
//Problem Details más errorCode y errorMessage. Los handlers la usan para no repetir código.
public static class ErrorResponseWriter
{
    public static async Task WriteAsync(
        HttpContext context,
        int status,
        string type,
        string title,
        string detail,
        string errorCode,
        string errorMessage,
        CancellationToken cancellationToken)
    {
        context.Response.StatusCode = status;

        await context.Response.WriteAsJsonAsync(new
        {
            type,
            title,
            status,
            detail,
            instance = context.Request.Path.Value,
            errorCode,
            errorMessage
        }, cancellationToken: cancellationToken);
    }
}

