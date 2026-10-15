using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Models.DTOs.OrderItems;

namespace PSite3.Controllers.Api;

[Route("api/orderitems")]
[ApiController]
public class OrderItemsApiController : ControllerBase
{
    private readonly ShopContext _context;

    public OrderItemsApiController(ShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var orderItems = await _context.OrderItems
            .OrderBy(oi => oi.IdCartItem)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(orderItems.Select(OrderItemDtoMapper.ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderItemDto>> GetOrderItem(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }

        return Ok(OrderItemDtoMapper.ToDto(orderItem));
    }

    [HttpPost]
    public async Task<ActionResult<OrderItemDto>> CreateOrderItem(CreateOrderItemDto createDto)
    {
        var orderItem = new OrderItem
        {
            OrderId = createDto.OrderId,
            ProductId = createDto.ProductId,
            Quantity = createDto.Quantity,
            Price = createDto.Price
        };

        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();

        var dto = OrderItemDtoMapper.ToDto(orderItem);
        return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.IdCartItem }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderItem(int id, UpdateOrderItemDto updateDto)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }

        orderItem.OrderId = updateDto.OrderId;
        orderItem.ProductId = updateDto.ProductId;
        orderItem.Quantity = updateDto.Quantity;
        orderItem.Price = updateDto.Price;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchOrderItem(int id, PatchOrderItemDto patchDto)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }

        if (patchDto.OrderId.HasValue) orderItem.OrderId = patchDto.OrderId.Value;
        if (patchDto.ProductId.HasValue) orderItem.ProductId = patchDto.ProductId.Value;
        if (patchDto.Quantity.HasValue) orderItem.Quantity = patchDto.Quantity.Value;
        if (patchDto.Price.HasValue) orderItem.Price = patchDto.Price.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }

        _context.OrderItems.Remove(orderItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
