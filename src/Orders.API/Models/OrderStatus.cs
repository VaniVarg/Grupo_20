namespace Orders.API.Models;

/// <summary>
/// Estados posibles de una orden. Se guardan como texto, tal como pide el modelo de datos.
/// </summary>
public static class OrderStatus
{
    public const string Pendiente = "Pendiente";
    public const string Confirmada = "Confirmada";
    public const string Enviada = "Enviada";
    public const string Entregada = "Entregada";
    public const string Cancelada = "Cancelada";

    /// <summary>Lista completa, útil para validar que un estado recibido exista.</summary>
    public static readonly string[] All = [Pendiente, Confirmada, Enviada, Entregada, Cancelada];
}
