using Cart.API.Clients;
using Cart.API.DTOs;
using Cart.API.Exceptions;
using Cart.API.Models;
using Cart.API.Persistence;

namespace Cart.API.Services;

public class CartService(ICartRepository repository, IProductsClient productsClient) : ICartService
{
    public async Task<CartResponse> GetCartAsync(Guid userId)
    {
        var cart = await GetCartOrThrowAsync(userId);
        return ToResponse(cart);
    }

    public async Task<CartResponse> AddItemAsync(Guid userId, AddCartItemRequest request)
    {
        // CRT-004: la cantidad tiene que ser mayor a 0.
        if (request is null || request.Cantidad <= 0)
            throw new ValidationException(ErrorCodes.CRT_004, "Cantidad inválida: debe ser mayor a 0.");

        if (request.ProductoId == Guid.Empty)
            throw new NotFoundException(ErrorCodes.CRT_002, "Producto no encontrado.");

        // Si el usuario todavía no tiene carrito, se crea en este momento.
        var cart = await repository.GetByUserIdAsync(userId)
            ?? new ShoppingCart { UsuarioId = userId };

        var item = cart.Items.FirstOrDefault(i => i.ProductoId == request.ProductoId);

        // El stock se compara contra lo que YA había en el carrito más lo que se agrega.
        await EnsureProductAndStockAsync(request.ProductoId, (item?.Cantidad ?? 0) + request.Cantidad);

        if (item is null)
            cart.Items.Add(new CartItem { ProductoId = request.ProductoId, Cantidad = request.Cantidad });
        else
            item.Cantidad += request.Cantidad; // el producto ya estaba: se suma la cantidad

        cart.FechaActualizacion = DateTime.UtcNow;
        await repository.SaveAsync(cart);

        return ToResponse(cart);
    }

    public async Task<CartResponse> UpdateItemAsync(Guid userId, Guid productId, UpdateCartItemRequest request)
    {
        if (request is null || request.Cantidad <= 0)
            throw new ValidationException(ErrorCodes.CRT_004, "Cantidad inválida: debe ser mayor a 0.");

        var cart = await GetCartOrThrowAsync(userId);
        var item = GetItemOrThrow(cart, productId);

        // Acá la cantidad REEMPLAZA a la anterior, por eso se compara directamente.
        await EnsureProductAndStockAsync(productId, request.Cantidad);

        item.Cantidad = request.Cantidad;
        cart.FechaActualizacion = DateTime.UtcNow;
        await repository.SaveAsync(cart);

        return ToResponse(cart);
    }

    public async Task RemoveItemAsync(Guid userId, Guid productId)
    {
        var cart = await GetCartOrThrowAsync(userId);
        var item = GetItemOrThrow(cart, productId);

        cart.Items.Remove(item);
        cart.FechaActualizacion = DateTime.UtcNow;
        await repository.SaveAsync(cart);
    }

    public async Task ClearCartAsync(Guid userId)
    {
        await GetCartOrThrowAsync(userId);
        await repository.DeleteAsync(userId);
    }

    private async Task<ShoppingCart> GetCartOrThrowAsync(Guid userId)
    {
        return await repository.GetByUserIdAsync(userId)
            ?? throw new NotFoundException(ErrorCodes.CRT_001, "Carrito no encontrado.");
    }

    private static CartItem GetItemOrThrow(ShoppingCart cart, Guid productId)
    {
        return cart.Items.FirstOrDefault(i => i.ProductoId == productId)
            ?? throw new NotFoundException(ErrorCodes.CRT_001, "El producto no está en el carrito.");
    }

    /// <summary>
    /// Consulta a Products.API: el producto tiene que existir (CRT-002) y su stock tiene
    /// que alcanzar para la cantidad pedida (CRT-003).
    /// </summary>
    private async Task EnsureProductAndStockAsync(Guid productId, int cantidadTotal)
    {
        var product = await productsClient.GetProductAsync(productId)
            ?? throw new NotFoundException(ErrorCodes.CRT_002, "Producto no encontrado.");

        if (product.Stock < cantidadTotal)
        {
            throw new BusinessRuleException(
                ErrorCodes.CRT_003,
                $"Stock insuficiente. Disponible: {product.Stock}, solicitado: {cantidadTotal}.");
        }
    }

    private static CartResponse ToResponse(ShoppingCart cart) => new()
    {
        UsuarioId = cart.UsuarioId,
        Items = cart.Items
            .Select(i => new CartItemResponse { ProductoId = i.ProductoId, Cantidad = i.Cantidad })
            .ToList(),
        FechaActualizacion = cart.FechaActualizacion
    };
}