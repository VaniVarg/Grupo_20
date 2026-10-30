using Products.API.Exceptions;
using Products.API.Swagger;
using Microsoft.AspNetCore.Mvc;
using Products.API.DTOs;
using Products.API.Services;

namespace Products.API.Controllers;

// Gestión del catálogo de productos.
[ApiController]
[Route("api/products")]
[Produces("application/json")]
public class ProductsController(IProductService service) : ControllerBase
{
    // Lista los productos, con filtros opcionales por categoría y nombre.
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
    [ErrorResponse(500, ErrorCodes.PRD_005, "Error interno al procesar el producto.")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll(
        [FromQuery] string? categoria,
        [FromQuery] string? nombre)
    {
        return Ok(await service.GetAllAsync(categoria, nombre));
    }

    // Obtiene un producto por su ID.
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ErrorResponse(404, ErrorCodes.PRD_001, "Producto no encontrado.")]
    [ErrorResponse(500, ErrorCodes.PRD_005, "Error interno al procesar el producto.")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id)
    {
        return Ok(await service.GetByIdAsync(id));
    }

    // Crea un nuevo producto.
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ErrorResponse(400, ErrorCodes.PRD_002, "Los datos del producto son inválidos. El precio debe ser mayor a 0.")]
    [ErrorResponse(409, ErrorCodes.PRD_003, "Ya existe un producto con ese nombre en la categoría 'Electrónica'.")]
    [ErrorResponse(500, ErrorCodes.PRD_005, "Error interno al procesar el producto.")]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] ProductRequest request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // Actualiza un producto existente.
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ErrorResponse(400, ErrorCodes.PRD_002, "Los datos del producto son inválidos. El precio debe ser mayor a 0.")]
    [ErrorResponse(404, ErrorCodes.PRD_001, "Producto no encontrado.")]
    [ErrorResponse(500, ErrorCodes.PRD_005, "Error interno al procesar el producto.")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResponse>> Update(Guid id, [FromBody] ProductRequest request)
    {
        return Ok(await service.UpdateAsync(id, request));
    }

    // Elimina un producto.
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ErrorResponse(404, ErrorCodes.PRD_001, "Producto no encontrado.")]
    [ErrorResponse(409, ErrorCodes.PRD_004, "El producto tiene órdenes activas y no puede eliminarse.")]
    [ErrorResponse(500, ErrorCodes.PRD_005, "Error interno al procesar el producto.")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
