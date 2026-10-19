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
    /// <summary>Crea una nueva orden. Valida usuario, productos y stock contra otros servicios.</summary>
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var created = await service.CreateAsync(request);
        return Created($"/api/orders/{created.Id}", created);
    }
}
