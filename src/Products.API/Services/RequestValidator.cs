using Products.API.Exceptions;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace Products.API.Services;

// Valida un objeto usando sus atributos de Data Annotations.
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
