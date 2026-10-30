using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.Carts;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class CartService : ICartService
{
    private readonly ICartRepository _carts;
    private readonly IUserRepository _users;
    private readonly IMapper _mapper;

    public CartService(ICartRepository carts, IUserRepository users, IMapper mapper)
    {
        _carts = carts;
        _users = users;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<CartDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _carts.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<CartDto>>.Ok(new PagedResult<CartDto>
        {
            Items = _mapper.Map<List<CartDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<CartDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var cart = await _carts.GetWithDetailsAsync(id, cancellationToken);

        return cart is null
            ? ServiceResult<CartDto>.NotFound($"Корзина с идентификатором {id} не найдена.")
            : ServiceResult<CartDto>.Ok(_mapper.Map<CartDto>(cart));
    }

    public async Task<ServiceResult<CartDto>> CreateAsync(
        CreateCartDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!await _users.AnyAsync(u => u.IdUser == dto.UserId, cancellationToken))
        {
            return ServiceResult<CartDto>.Validation(
                $"Пользователь с идентификатором {dto.UserId} не найден.");
        }

        var cart = _mapper.Map<Cart>(dto);

        await _carts.AddAsync(cart, cancellationToken);
        await _carts.SaveChangesAsync(cancellationToken);

        var created = await _carts.GetWithDetailsAsync(cart.IdCart, cancellationToken)
                      ?? cart;

        return ServiceResult<CartDto>.Created(_mapper.Map<CartDto>(created));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateCartDto dto,
        CancellationToken cancellationToken = default)
    {
        var cart = await _carts.GetByIdAsync(id, cancellationToken);
        if (cart is null)
        {
            return ServiceResult<bool>.NotFound($"Корзина с идентификатором {id} не найдена.");
        }

        if (!await _users.AnyAsync(u => u.IdUser == dto.UserId, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Пользователь с идентификатором {dto.UserId} не найден.");
        }

        _mapper.Map(dto, cart);
        _carts.Update(cart);
        await _carts.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchCartDto dto,
        CancellationToken cancellationToken = default)
    {
        var cart = await _carts.GetByIdAsync(id, cancellationToken);
        if (cart is null)
        {
            return ServiceResult<bool>.NotFound($"Корзина с идентификатором {id} не найдена.");
        }

        if (dto.UserId.HasValue
            && !await _users.AnyAsync(u => u.IdUser == dto.UserId.Value, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Пользователь с идентификатором {dto.UserId.Value} не найден.");
        }

        _mapper.Map(dto, cart);
        _carts.Update(cart);
        await _carts.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var cart = await _carts.GetByIdAsync(id, cancellationToken);
        if (cart is null)
        {
            return ServiceResult<bool>.NotFound($"Корзина с идентификатором {id} не найдена.");
        }

        if (await _carts.HasDependenciesAsync(id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "Невозможно удалить корзину: в ней есть товары.");
        }

        _carts.Remove(cart);
        await _carts.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }
}
