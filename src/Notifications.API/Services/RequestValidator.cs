using Notifications.API.Exceptions;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace Notifications.API.Services;

//Valida un objeto con sus Data Annotations y, si hay errores, lanza nuestra ValidationException con el código del catálogo. El alias DataAnnotations evita confundirla con la ValidationException que trae .NET.

public static class RequestValidator
{
    public static void Validate(object? instance, string errorCode, string defaultMessage)
    {
        if (instance is null)
            throw new ValidationException(errorCode, defaultMessage);

        var results = new List<DataAnnotations.ValidationResult>();
        var context = new DataAnnotations.ValidationContext(instance);

        if (DataAnnotations.Validator.TryValidateObject(instance, context, results, validateAllProperties: true))
            return;

        var detalle = string.Join("; ", results.Select(r => r.ErrorMessage));
        throw new ValidationException(errorCode, $"{defaultMessage} {detalle}");
    }
}