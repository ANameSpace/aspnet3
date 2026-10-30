using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.CartItems;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/cartitems")]
[ApiController]
public class CartItemsApiController : ApiControllerBase
{
    private readonly ICartItemService _cartItems;

    public CartItemsApiController(ICartItemService cartItems)
    {
        _cartItems = cartItems;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _cartItems.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<CartItemDto>> GetCartItem(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _cartItems.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> CreateCartItem(
        CreateCartItemDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _cartItems.CreateAsync(dto, cancellationToken),
            nameof(GetCartItem),
            cartItem => cartItem.IdCartItem);

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateCartItem(
        int id,
        UpdateCartItemDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _cartItems.UpdateAsync(id, dto, cancellationToken));

    [HttpPatch("{id}")]
    public async Task<ActionResult<bool>> PatchCartItem(
        int id,
        PatchCartItemDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _cartItems.PatchAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteCartItem(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _cartItems.DeleteAsync(id, cancellationToken));
}
