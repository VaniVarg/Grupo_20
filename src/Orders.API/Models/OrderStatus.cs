namespace Orders.API.Models;

/// <summary>
/// Estados posibles de una orden y las transiciones permitidas entre ellos.
/// Se guardan como texto, tal como pide el modelo de datos.
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

    // Para cada estado, a cuáles se puede pasar. Entregada y Cancelada son estados finales.
    private static readonly Dictionary<string, string[]> Transitions = new()
    {
        [Pendiente] = [Confirmada, Cancelada],
        [Confirmada] = [Enviada, Cancelada],
        [Enviada] = [Entregada],
        [Entregada] = [],
        [Cancelada] = []
    };

    /// <summary>True si una orden en el estado "from" puede pasar al estado "to".</summary>
    public static bool CanTransition(string from, string to) =>
        Transitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
}
