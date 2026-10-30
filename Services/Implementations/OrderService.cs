using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.Orders;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;
    private readonly IUserRepository _users;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orders, IUserRepository users, IMapper mapper)
    {
        _orders = orders;
        _users = users;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<OrderDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _orders.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<OrderDto>>.Ok(new PagedResult<OrderDto>
        {
            Items = _mapper.Map<List<OrderDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<OrderDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetWithDetailsAsync(id, cancellationToken);

        return order is null
            ? ServiceResult<OrderDto>.NotFound($"Заказ с идентификатором {id} не найден.")
            : ServiceResult<OrderDto>.Ok(_mapper.Map<OrderDto>(order));
    }

    public async Task<ServiceResult<OrderDto>> CreateAsync(
        CreateOrderDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!await _users.AnyAsync(u => u.IdUser == dto.UserId, cancellationToken))
        {
            return ServiceResult<OrderDto>.Validation(
                $"Пользователь с идентификатором {dto.UserId} не найден.");
        }

        var order = _mapper.Map<Order>(dto);
        order.OrderDate = DateTime.UtcNow;

        await _orders.AddAsync(order, cancellationToken);
        await _orders.SaveChangesAsync(cancellationToken);

        var created = await _orders.GetWithDetailsAsync(order.IdOrder, cancellationToken)
                      ?? order;

        return ServiceResult<OrderDto>.Created(_mapper.Map<OrderDto>(created));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateOrderDto dto,
        CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return ServiceResult<bool>.NotFound($"Заказ с идентификатором {id} не найден.");
        }

        if (!await _users.AnyAsync(u => u.IdUser == dto.UserId, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Пользователь с идентификатором {dto.UserId} не найден.");
        }

        _mapper.Map(dto, order);
        _orders.Update(order);
        await _orders.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchOrderDto dto,
        CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return ServiceResult<bool>.NotFound($"Заказ с идентификатором {id} не найден.");
        }

        if (dto.UserId.HasValue
            && !await _users.AnyAsync(u => u.IdUser == dto.UserId.Value, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Пользователь с идентификатором {dto.UserId.Value} не найден.");
        }

        _mapper.Map(dto, order);
        _orders.Update(order);
        await _orders.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return ServiceResult<bool>.NotFound($"Заказ с идентификатором {id} не найден.");
        }

        if (await _orders.HasDependenciesAsync(id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "Невозможно удалить заказ: в нём есть позиции или резервирования.");
        }

        _orders.Remove(order);
        await _orders.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }
}
