using Microsoft.AspNetCore.Mvc;
using Orders.API.DTOs;
using Orders.API.Services;

namespace Orders.API.Controllers;

/// <summary>
/// Gestión de órdenes de compra.
/// </summary>
[ApiController]
[Route("api/orders")]
[Produces("application/json")]
public class OrdersController(IOrderService service) : ControllerBase
{
    /// <summary>Lista las órdenes, con filtro opcional por usuario (?usuarioId=).</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll([FromQuery] Guid? usuarioId)
    {
        return Ok(await service.GetAllAsync(usuarioId));
    }

    /// <summary>Obtiene el detalle de una orden.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetById(Guid id)
    {
        return Ok(await service.GetByIdAsync(id));
    }

    /// <summary>Crea una nueva orden. Valida usuario, productos y stock contra otros servicios.</summary>
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var created = await service.CreateAsync(request);
        return Created($"/api/orders/{created.Id}", created);
    }

    /// <summary>Cambia el estado de una orden. Solo se permiten ciertas transiciones.</summary>
    [HttpPut("{id}/status")]
    public async Task<ActionResult<UpdateOrderStatusResponse>> UpdateStatus(
        Guid id, [FromBody] UpdateOrderStatusRequest request)
    {
        return Ok(await service.UpdateStatusAsync(id, request));
    }
}
