namespace Cart.API.Clients;

/// <summary>
/// Los pocos datos de un producto que necesita este servicio. Es una clase propia de
/// Cart.API (no se comparte código entre microservicios): si Products.API agrega campos,
/// acá solo se declaran los que se usan.
/// </summary>
public record ProductInfo(Guid Id, string Nombre, decimal Precio, int Stock);