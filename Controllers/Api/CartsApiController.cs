using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Models.DTOs.Carts;

namespace PSite3.Controllers.Api;

[Route("api/carts")]
[ApiController]
public class CartsApiController : ControllerBase
{
    private readonly ShopContext _context;

    public CartsApiController(ShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartDto>>> GetCarts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var carts = await _context.Carts
            .OrderBy(c => c.IdCart)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(carts.Select(CartDtoMapper.ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartDto>> GetCart(int id)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart == null)
        {
            return NotFound();
        }

        return Ok(CartDtoMapper.ToDto(cart));
    }

    [HttpPost]
    public async Task<ActionResult<CartDto>> CreateCart(CreateCartDto createDto)
    {
        var cart = new Cart
        {
            UserId = createDto.UserId
        };

        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();

        var dto = CartDtoMapper.ToDto(cart);
        return CreatedAtAction(nameof(GetCart), new { id = cart.IdCart }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCart(int id, UpdateCartDto updateDto)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart == null)
        {
            return NotFound();
        }

        cart.UserId = updateDto.UserId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCart(int id, PatchCartDto patchDto)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart == null)
        {
            return NotFound();
        }

        if (patchDto.UserId.HasValue) cart.UserId = patchDto.UserId.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCart(int id)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart == null)
        {
            return NotFound();
        }

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
