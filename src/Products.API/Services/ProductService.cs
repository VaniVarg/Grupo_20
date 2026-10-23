using Products.API.Clients;
using Products.API.DTOs;
using Products.API.Exceptions;
using Products.API.Models;
using Products.API.Persistence;

namespace Products.API.Services;

public class ProductService(IProductRepository repository, IOrdersClient ordersClient) : IProductService
{
    public async Task<IEnumerable<ProductResponse>> GetAllAsync(string? categoria, string? nombre)
    {
        var products = await repository.GetAllAsync(categoria, nombre);
        return products.Select(ToResponse).ToList();
    }

    public async Task<ProductResponse> GetByIdAsync(Guid id)
    {
        var product = await GetOrThrowAsync(id);
        return ToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(ProductRequest request)
    {
        RequestValidator.Validate(request, ErrorCodes.PRD_002, "Los datos del producto son inválidos.");

        var nombre = request.Nombre.Trim();
        var categoria = request.Categoria.Trim();

        if (await repository.ExistsByNameAndCategoryAsync(nombre, categoria))
        {
            throw new BusinessRuleException(
                ErrorCodes.PRD_003,
                $"Ya existe un producto con ese nombre en la categoría '{categoria}'.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Nombre = nombre,
            Descripcion = request.Descripcion?.Trim(),
            Precio = request.Precio,
            Stock = request.Stock,
            Categoria = categoria,
            FechaCreacion = DateTime.UtcNow
        };

        await repository.AddAsync(product);
        return ToResponse(product);
    }

    public async Task<ProductResponse> UpdateAsync(Guid id, ProductRequest request)
    {
        RequestValidator.Validate(request, ErrorCodes.PRD_002, "Los datos del producto son inválidos.");

        var product = await GetOrThrowAsync(id);

        product.Nombre = request.Nombre.Trim();
        product.Descripcion = request.Descripcion?.Trim();
        product.Precio = request.Precio;
        product.Stock = request.Stock;
        product.Categoria = request.Categoria.Trim();

        await repository.UpdateAsync(product);
        return ToResponse(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await GetOrThrowAsync(id);

        // Llamada HTTP a Orders.API: no se puede borrar un producto con órdenes Pendiente o
        // Confirmada (PRD-004). Si Orders.API no responde, la excepción sube y el handler genérico
        // devuelve PRD-005: ante la duda NO se borra.
        if (await ordersClient.HasActiveOrdersAsync(id))
        {
            throw new BusinessRuleException(
                ErrorCodes.PRD_004,
                "El producto tiene órdenes activas y no puede eliminarse.");
        }

        await repository.DeleteAsync(id);
    }

    private async Task<Product> GetOrThrowAsync(Guid id)
    {
        return await repository.GetByIdAsync(id)
            ?? throw new NotFoundException(ErrorCodes.PRD_001, "Producto no encontrado.");
    }

    private static ProductResponse ToResponse(Product p) => new()
    {
        Id = p.Id,
        Nombre = p.Nombre,
        Descripcion = p.Descripcion,
        Precio = p.Precio,
        Stock = p.Stock,
        Categoria = p.Categoria,
        FechaCreacion = p.FechaCreacion
    };
}
