using System.Net;
using System.Net.Http.Json;

namespace Cart.API.Clients;

public class ProductsClient(HttpClient httpClient, ILogger<ProductsClient> logger) : IProductsClient
{
    public async Task<ProductInfo?> GetProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"api/products/{id}", cancellationToken);

        // Siempre se revisa el status HTTP ANTES de leer el body.
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Products.API respondió {StatusCode} al consultar el producto {ProductId}",
                (int)response.StatusCode, id);
            throw new HttpRequestException($"Products.API respondió {(int)response.StatusCode}.");
        }

        return await response.Content.ReadFromJsonAsync<ProductInfo>(cancellationToken);
    }
}