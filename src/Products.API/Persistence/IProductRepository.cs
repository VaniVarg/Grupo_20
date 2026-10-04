using Products.API.Models;

namespace Products.API.Persistence;

// Contrato de acceso a datos de productos
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(string? categoria, string? nombre);
    Task<Product?> GetByIdAsync(Guid id);
    Task<bool> ExistsByNameAndCategoryAsync(string nombre, string categoria);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
}