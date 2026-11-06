namespace Products.API.DTOs;
// Representación de un producto que devuelve la API.

public class ProductResponse
{
    // Identificador único del producto. <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }
    // Nombre del producto.<example>Notebook Dell XPS 15</example>
    public string Nombre { get; set; } = string.Empty;
    // Descripción opcional. <example>Laptop 15 pulgadas, 32GB RAM</example>
    public string? Descripcion { get; set; }
    // Precio unitario. <example>1500.00</example>
    public decimal Precio { get; set; }
    // Unidades disponibles.<example>10</example>
    public int Stock { get; set; }
    // Categoría del producto. <example>Electrónica</example>
    public string Categoria { get; set; } = string.Empty;
    //Fecha de creación en UTC.<example>2026-10-28T12:00:00Z</example>
    public DateTime FechaCreacion { get; set; }
}
