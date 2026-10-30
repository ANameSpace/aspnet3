using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.Carts;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/carts")]
[ApiController]
public class CartsApiController : ApiControllerBase
{
    private readonly ICartService _carts;

    public CartsApiController(ICartService carts)
    {
        _carts = carts;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartDto>>> GetCarts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _carts.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<CartDto>> GetCart(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _carts.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<CartDto>> CreateCart(
        CreateCartDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _carts.CreateAsync(dto, cancellationToken),
            nameof(GetCart),
            cart => cart.IdCart);

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateCart(
        int id,
        UpdateCartDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _carts.UpdateAsync(id, dto, cancellationToken));

    [HttpPatch("{id}")]
    public async Task<ActionResult<bool>> PatchCart(
        int id,
        PatchCartDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _carts.PatchAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteCart(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _carts.DeleteAsync(id, cancellationToken));
}
