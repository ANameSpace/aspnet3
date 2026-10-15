using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Models.DTOs.CartItems;

namespace PSite3.Controllers.Api;

[Route("api/cartitems")]
[ApiController]
public class CartItemsApiController : ControllerBase
{
    private readonly ShopContext _context;

    public CartItemsApiController(ShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var cartItems = await _context.CartItems
            .OrderBy(ci => ci.IdCartItem)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(cartItems.Select(CartItemDtoMapper.ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartItemDto>> GetCartItem(int id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem == null)
        {
            return NotFound();
        }

        return Ok(CartItemDtoMapper.ToDto(cartItem));
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> CreateCartItem(CreateCartItemDto createDto)
    {
        var cartItem = new CartItem
        {
            CartId = createDto.CartId,
            ProductId = createDto.ProductId,
            Quantity = createDto.Quantity
        };

        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        var dto = CartItemDtoMapper.ToDto(cartItem);
        return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.IdCartItem }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCartItem(int id, UpdateCartItemDto updateDto)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem == null)
        {
            return NotFound();
        }

        cartItem.CartId = updateDto.CartId;
        cartItem.ProductId = updateDto.ProductId;
        cartItem.Quantity = updateDto.Quantity;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCartItem(int id, PatchCartItemDto patchDto)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem == null)
        {
            return NotFound();
        }

        if (patchDto.CartId.HasValue) cartItem.CartId = patchDto.CartId.Value;
        if (patchDto.ProductId.HasValue) cartItem.ProductId = patchDto.ProductId.Value;
        if (patchDto.Quantity.HasValue) cartItem.Quantity = patchDto.Quantity.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCartItem(int id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem == null)
        {
            return NotFound();
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
