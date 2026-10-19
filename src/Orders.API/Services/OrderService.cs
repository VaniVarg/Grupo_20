using Orders.API.Clients;
using Orders.API.DTOs;
using Orders.API.Exceptions;
using Orders.API.Models;
using Orders.API.Persistence;

namespace Orders.API.Services;

public class OrderService(
    IOrderRepository repository,
    IUsersClient usersClient,
    IProductsClient productsClient,
    ILogger<OrderService> logger) : IOrderService
{
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
    {
        // 1) Datos de entrada (ORD-002).
        RequestValidator.Validate(request, ErrorCodes.ORD_002, "Los datos de la orden son inválidos.");

        if (request.UsuarioId == Guid.Empty)
            throw new ValidationException(ErrorCodes.ORD_002, "Los datos de la orden son inválidos. El usuario es obligatorio.");

        // RequestValidator no recorre los elementos de una lista: los revisamos a mano.
        if (request.Items.Any(i => i.ProductoId == Guid.Empty || i.Cantidad <= 0))
        {
            throw new ValidationException(
                ErrorCodes.ORD_002,
                "Los datos de la orden son inválidos. Cada item necesita un producto y una cantidad mayor a 0.");
        }

        // 2) El usuario tiene que existir en Users.API (ORD-003).
        if (!await usersClient.ExistsAsync(request.UsuarioId))
            throw new NotFoundException(ErrorCodes.ORD_003, "Usuario no encontrado.");

        // 3) Si el mismo producto viene repetido, se agrupa y se suman las cantidades:
        //    así no se puede esquivar el control de stock pidiéndolo en dos líneas.
        var requested = request.Items
            .GroupBy(i => i.ProductoId)
            .Select(g => new { ProductoId = g.Key, Cantidad = g.Sum(i => i.Cantidad) });

        var items = new List<OrderItem>();
        foreach (var line in requested)
        {
            // Los productos tienen que existir en Products.API (ORD-004)...
            var product = await productsClient.GetProductAsync(line.ProductoId)
                ?? throw new NotFoundException(ErrorCodes.ORD_004, $"Producto '{line.ProductoId}' no encontrado.");

            // ...y tiene que alcanzar el stock (ORD-005).
            if (product.Stock < line.Cantidad)
            {
                throw new BusinessRuleException(
                    ErrorCodes.ORD_005,
                    $"Stock insuficiente para '{product.Nombre}'. Disponible: {product.Stock}, solicitado: {line.Cantidad}.");
            }

            // El precio se guarda en la orden al momento de comprar.
            items.Add(new OrderItem
            {
                ProductoId = line.ProductoId,
                Cantidad = line.Cantidad,
                PrecioUnitario = product.Precio
            });
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UsuarioId = request.UsuarioId,
            Items = items,
            Total = items.Sum(i => i.Cantidad * i.PrecioUnitario),
            Estado = OrderStatus.Pendiente,
            FechaCreacion = DateTime.UtcNow
        };

        await repository.AddAsync(order);

        logger.LogInformation("Orden {OrderId} creada para el usuario {UsuarioId} por un total de {Total}",
            order.Id, order.UsuarioId, order.Total);

        return ToResponse(order);
    }

    private static OrderResponse ToResponse(Order o) => new()
    {
        Id = o.Id,
        UsuarioId = o.UsuarioId,
        Items = o.Items
            .Select(i => new OrderItemResponse
            {
                ProductoId = i.ProductoId,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario
            })
            .ToList(),
        Total = o.Total,
        Estado = o.Estado,
        FechaCreacion = o.FechaCreacion
    };
}
