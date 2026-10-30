using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.Orders;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/orders")]
[ApiController]
public class OrdersApiController : ApiControllerBase
{
    private readonly IOrderService _orders;

    public OrdersApiController(IOrderService orders)
    {
        _orders = orders;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _orders.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _orders.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(
        CreateOrderDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _orders.CreateAsync(dto, cancellationToken),
            nameof(GetOrder),
            order => order.IdOrder);

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateOrder(
        int id,
        UpdateOrderDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _orders.UpdateAsync(id, dto, cancellationToken));

    [HttpPatch("{id}")]
    public async Task<ActionResult<bool>> PatchOrder(
        int id,
        PatchOrderDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _orders.PatchAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteOrder(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _orders.DeleteAsync(id, cancellationToken));
}
