namespace Products.API.Models
{
    // Entidad de dominio del catálogo de productos.
    public class Product
    {
        // Identificador único del producto
        public Guid Id { get; set; }

        // Nombre del producto (requerido, máx. 100 caracteres)
        public string Nombre { get; set; } = string.Empty;

        // Descripción opcional (máx. 500 caracteres)
        public string? Descripcion { get; set; }

        // Precio unitario, siempre mayor a 0
        public decimal Precio { get; set; }

        // Unidades disponibles, mayor o igual a 0
        public int Stock { get; set; }

        // Categoría informativa (no se valida contra una lista fija)
        public string Categoria { get; set; } = string.Empty;

        // Fecha de creación en UTC, asignada automáticamente
        public DateTime FechaCreacion { get; set; }
    }
}
