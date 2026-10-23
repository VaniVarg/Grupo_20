namespace Products.API.Clients;

// Un item de una orden: solo nos interesa a qué producto se refiere.
public record OrderItemInfo(Guid ProductoId);

// Los pocos datos de una orden que necesita Products.API. Es una clase propia de este servicio: no se comparte código entre microservicios.
public record OrderInfo(Guid Id, string Estado, List<OrderItemInfo> Items);
