using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.CartItems;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItems;
    private readonly ICartRepository _carts;
    private readonly IProductRepository _products;
    private readonly IMapper _mapper;

    public CartItemService(
        ICartItemRepository cartItems,
        ICartRepository carts,
        IProductRepository products,
        IMapper mapper)
    {
        _cartItems = cartItems;
        _carts = carts;
        _products = products;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<CartItemDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _cartItems.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<CartItemDto>>.Ok(new PagedResult<CartItemDto>
        {
            Items = _mapper.Map<List<CartItemDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<CartItemDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var cartItem = await _cartItems.GetWithDetailsAsync(id, cancellationToken);

        return cartItem is null
            ? ServiceResult<CartItemDto>.NotFound($"Позиция корзины с идентификатором {id} не найдена.")
            : ServiceResult<CartItemDto>.Ok(_mapper.Map<CartItemDto>(cartItem));
    }

    public async Task<ServiceResult<CartItemDto>> CreateAsync(
        CreateCartItemDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!await _carts.AnyAsync(c => c.IdCart == dto.CartId, cancellationToken))
        {
            return ServiceResult<CartItemDto>.Validation(
                $"Корзина с идентификатором {dto.CartId} не найдена.");
        }

        if (!await _products.AnyAsync(p => p.IdProduct == dto.ProductId, cancellationToken))
        {
            return ServiceResult<CartItemDto>.Validation(
                $"Товар с идентификатором {dto.ProductId} не найден.");
        }

        if (await _cartItems.PairExistsAsync(dto.CartId, dto.ProductId, null, cancellationToken))
        {
            return ServiceResult<CartItemDto>.Conflict(
                "В корзине уже есть позиция с указанным товаром.");
        }

        var cartItem = _mapper.Map<CartItem>(dto);

        await _cartItems.AddAsync(cartItem, cancellationToken);
        await _cartItems.SaveChangesAsync(cancellationToken);

        var created = await _cartItems.GetWithDetailsAsync(cartItem.IdCartItem, cancellationToken)
                      ?? cartItem;

        return ServiceResult<CartItemDto>.Created(_mapper.Map<CartItemDto>(created));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateCartItemDto dto,
        CancellationToken cancellationToken = default)
    {
        var cartItem = await _cartItems.GetByIdAsync(id, cancellationToken);
        if (cartItem is null)
        {
            return ServiceResult<bool>.NotFound($"Позиция корзины с идентификатором {id} не найдена.");
        }

        if (!await _carts.AnyAsync(c => c.IdCart == dto.CartId, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Корзина с идентификатором {dto.CartId} не найдена.");
        }

        if (!await _products.AnyAsync(p => p.IdProduct == dto.ProductId, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Товар с идентификатором {dto.ProductId} не найден.");
        }

        if (await _cartItems.PairExistsAsync(dto.CartId, dto.ProductId, id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "В корзине уже есть позиция с указанным товаром.");
        }

        _mapper.Map(dto, cartItem);
        _cartItems.Update(cartItem);
        await _cartItems.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchCartItemDto dto,
        CancellationToken cancellationToken = default)
    {
        var cartItem = await _cartItems.GetByIdAsync(id, cancellationToken);
        if (cartItem is null)
        {
            return ServiceResult<bool>.NotFound($"Позиция корзины с идентификатором {id} не найдена.");
        }

        if (dto.CartId.HasValue
            && !await _carts.AnyAsync(c => c.IdCart == dto.CartId.Value, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Корзина с идентификатором {dto.CartId.Value} не найдена.");
        }

        if (dto.ProductId.HasValue
            && !await _products.AnyAsync(p => p.IdProduct == dto.ProductId.Value, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Товар с идентификатором {dto.ProductId.Value} не найден.");
        }

        var cartId = dto.CartId ?? cartItem.CartId;
        var productId = dto.ProductId ?? cartItem.ProductId;

        if (await _cartItems.PairExistsAsync(cartId, productId, id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "В корзине уже есть позиция с указанным товаром.");
        }

        _mapper.Map(dto, cartItem);
        _cartItems.Update(cartItem);
        await _cartItems.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var cartItem = await _cartItems.GetByIdAsync(id, cancellationToken);
        if (cartItem is null)
        {
            return ServiceResult<bool>.NotFound($"Позиция корзины с идентификатором {id} не найдена.");
        }

        _cartItems.Remove(cartItem);
        await _cartItems.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }
}
