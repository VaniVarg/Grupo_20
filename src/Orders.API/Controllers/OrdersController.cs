using Orders.API.Exceptions;
using Orders.API.Swagger;
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
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    [ErrorResponse(500, ErrorCodes.ORD_007, "Error interno al procesar la orden.")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll([FromQuery] Guid? usuarioId)
    {
        return Ok(await service.GetAllAsync(usuarioId));
    }

    /// <summary>Obtiene el detalle de una orden.</summary>
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ErrorResponse(404, ErrorCodes.ORD_001, "Orden no encontrada.")]
    [ErrorResponse(500, ErrorCodes.ORD_007, "Error interno al procesar la orden.")]
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetById(Guid id)
    {
        return Ok(await service.GetByIdAsync(id));
    }

    /// <summary>Crea una nueva orden. Valida usuario, productos y stock contra otros servicios.</summary>
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ErrorResponse(400, ErrorCodes.ORD_002, "Los datos de la orden son inválidos. La orden debe tener al menos un item.")]
    [ErrorResponse(404, ErrorCodes.ORD_003, "Usuario no encontrado.")]
    [ErrorResponse(404, ErrorCodes.ORD_004, "Producto '3fa85f64-5717-4562-b3fc-2c963f66afa6' no encontrado.")]
    [ErrorResponse(422, ErrorCodes.ORD_005, "Stock insuficiente para 'Notebook Dell XPS 15'. Disponible: 1, solicitado: 5.")]
    [ErrorResponse(500, ErrorCodes.ORD_007, "Error interno al procesar la orden.")]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var created = await service.CreateAsync(request);
        return Created($"/api/orders/{created.Id}", created);
    }

    /// <summary>Cambia el estado de una orden. Solo se permiten ciertas transiciones.</summary>
    [ProducesResponseType(typeof(UpdateOrderStatusResponse), StatusCodes.Status200OK)]
    [ErrorResponse(400, ErrorCodes.ORD_002, "Estado inválido. Valores permitidos: Pendiente, Confirmada, Enviada, Entregada, Cancelada.")]
    [ErrorResponse(404, ErrorCodes.ORD_001, "Orden no encontrada.")]
    [ErrorResponse(409, ErrorCodes.ORD_006, "Una orden en estado 'Entregada' no puede pasar a 'Pendiente'.")]
    [ErrorResponse(500, ErrorCodes.ORD_007, "Error interno al procesar la orden.")]
    [HttpPut("{id}/status")]
    public async Task<ActionResult<UpdateOrderStatusResponse>> UpdateStatus(
        Guid id, [FromBody] UpdateOrderStatusRequest request)
    {
        return Ok(await service.UpdateStatusAsync(id, request));
    }
}
