using Microsoft.AspNetCore.Mvc;
using Products.API.DTOs;
using Products.API.Services;

namespace Products.API.Controllers;

// Gestión del catálogo de productos
[ApiController]
[Route("api/products")]
[Produces("application/json")]
public class ProductsController(IProductService service) : ControllerBase
{
    // Lista los productos, con filtros opcionales.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll(
        [FromQuery] string? categoria,
        [FromQuery] string? nombre)
    {
        return Ok(await service.GetAllAsync(categoria, nombre));
    }

    // Obtiene un producto por su ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id)
    {
        return Ok(await service.GetByIdAsync(id));
    }

    // Crea un nuevo producto
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] ProductRequest request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // Actualiza un producto existente
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResponse>> Update(Guid id, [FromBody] ProductRequest request)
    {
        return Ok(await service.UpdateAsync(id, request));
    }

    // Elimina un producto
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}