using Products.API.DTOs;

namespace Products.API.Services;

// Lógica de negocio de productos
public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync(string? categoria, string? nombre);
    Task<ProductResponse> GetByIdAsync(Guid id);
    Task<ProductResponse> CreateAsync(ProductRequest request);
    Task<ProductResponse> UpdateAsync(Guid id, ProductRequest request);
    Task DeleteAsync(Guid id);
}
