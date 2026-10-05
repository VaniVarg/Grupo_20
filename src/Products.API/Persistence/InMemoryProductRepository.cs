using System.Collections.Concurrent;
using Products.API.Models;
namespace Products.API.Persistence
{
   // Implementación temporal en memoria para poder desarrollar y probar el servicio.
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly ConcurrentDictionary<Guid, Product> _products = new();

        public Task<IEnumerable<Product>> GetAllAsync(string? categoria, string? nombre)
        {
            IEnumerable<Product> query = _products.Values;

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult<IEnumerable<Product>>(query.OrderBy(p => p.FechaCreacion).ToList());
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            _products.TryGetValue(id, out var product);
            return Task.FromResult<Product?>(product);
        }

        public Task<bool> ExistsByNameAndCategoryAsync(string nombre, string categoria)
        {
            var exists = _products.Values.Any(p =>
                p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) &&
                p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(exists);
        }

        public Task AddAsync(Product product)
        {
            _products[product.Id] = product;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Product product)
        {
            _products[product.Id] = product;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _products.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
