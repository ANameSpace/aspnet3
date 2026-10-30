using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.OrderItems;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/orderitems")]
[ApiController]
public class OrderItemsApiController : ApiControllerBase
{
    private readonly IOrderItemService _orderItems;

    public OrderItemsApiController(IOrderItemService orderItems)
    {
        _orderItems = orderItems;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _orderItems.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderItemDto>> GetOrderItem(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _orderItems.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<OrderItemDto>> CreateOrderItem(
        CreateOrderItemDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _orderItems.CreateAsync(dto, cancellationToken),
            nameof(GetOrderItem),
            orderItem => orderItem.IdCartItem);

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateOrderItem(
        int id,
        UpdateOrderItemDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _orderItems.UpdateAsync(id, dto, cancellationToken));

    [HttpPatch("{id}")]
    public async Task<ActionResult<bool>> PatchOrderItem(
        int id,
        PatchOrderItemDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _orderItems.PatchAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteOrderItem(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _orderItems.DeleteAsync(id, cancellationToken));
}
