namespace Products.API.Exceptions
{
    // Catálogo de códigos de error de Products.API.
    // Se usan constantes para no repetir strings sueltos en el código.
    public class ErrorCodes
    {
        //404 - Producto no encontrado (GET/PUT/DELETE con ID inexistente)
        public const string PRD_001 = "PRD-001";

        //400 - Datos del producto inválidos (POST/PUT)
        public const string PRD_002 = "PRD-002";

        //409 - Ya existe un producto con ese nombre en la categoría (POST)
        public const string PRD_003 = "PRD-003";

        //409 - El producto tiene órdenes activas y no puede eliminarse (DELETE)
        public const string PRD_004 = "PRD-004";

        //500 - Error interno inesperado
        public const string PRD_005 = "PRD-005";
    }
}
