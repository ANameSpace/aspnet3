using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.OrderItems;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItems;
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly IMapper _mapper;

    public OrderItemService(
        IOrderItemRepository orderItems,
        IOrderRepository orders,
        IProductRepository products,
        IMapper mapper)
    {
        _orderItems = orderItems;
        _orders = orders;
        _products = products;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<OrderItemDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _orderItems.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<OrderItemDto>>.Ok(new PagedResult<OrderItemDto>
        {
            Items = _mapper.Map<List<OrderItemDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<OrderItemDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var orderItem = await _orderItems.GetWithDetailsAsync(id, cancellationToken);

        return orderItem is null
            ? ServiceResult<OrderItemDto>.NotFound($"Позиция заказа с идентификатором {id} не найдена.")
            : ServiceResult<OrderItemDto>.Ok(_mapper.Map<OrderItemDto>(orderItem));
    }

    public async Task<ServiceResult<OrderItemDto>> CreateAsync(
        CreateOrderItemDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!await _orders.AnyAsync(o => o.IdOrder == dto.OrderId, cancellationToken))
        {
            return ServiceResult<OrderItemDto>.Validation(
                $"Заказ с идентификатором {dto.OrderId} не найден.");
        }

        if (!await _products.AnyAsync(p => p.IdProduct == dto.ProductId, cancellationToken))
        {
            return ServiceResult<OrderItemDto>.Validation(
                $"Товар с идентификатором {dto.ProductId} не найден.");
        }

        if (await _orderItems.PairExistsAsync(dto.OrderId, dto.ProductId, null, cancellationToken))
        {
            return ServiceResult<OrderItemDto>.Conflict(
                "В заказе уже есть позиция с указанным товаром.");
        }

        var orderItem = _mapper.Map<OrderItem>(dto);

        await _orderItems.AddAsync(orderItem, cancellationToken);
        await _orderItems.SaveChangesAsync(cancellationToken);

        var created = await _orderItems.GetWithDetailsAsync(orderItem.IdCartItem, cancellationToken)
                      ?? orderItem;

        return ServiceResult<OrderItemDto>.Created(_mapper.Map<OrderItemDto>(created));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateOrderItemDto dto,
        CancellationToken cancellationToken = default)
    {
        var orderItem = await _orderItems.GetByIdAsync(id, cancellationToken);
        if (orderItem is null)
        {
            return ServiceResult<bool>.NotFound($"Позиция заказа с идентификатором {id} не найдена.");
        }

        if (!await _orders.AnyAsync(o => o.IdOrder == dto.OrderId, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Заказ с идентификатором {dto.OrderId} не найден.");
        }

        if (!await _products.AnyAsync(p => p.IdProduct == dto.ProductId, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Товар с идентификатором {dto.ProductId} не найден.");
        }

        if (await _orderItems.PairExistsAsync(dto.OrderId, dto.ProductId, id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "В заказе уже есть позиция с указанным товаром.");
        }

        _mapper.Map(dto, orderItem);
        _orderItems.Update(orderItem);
        await _orderItems.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchOrderItemDto dto,
        CancellationToken cancellationToken = default)
    {
        var orderItem = await _orderItems.GetByIdAsync(id, cancellationToken);
        if (orderItem is null)
        {
            return ServiceResult<bool>.NotFound($"Позиция заказа с идентификатором {id} не найдена.");
        }

        if (dto.OrderId.HasValue
            && !await _orders.AnyAsync(o => o.IdOrder == dto.OrderId.Value, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Заказ с идентификатором {dto.OrderId.Value} не найден.");
        }

        if (dto.ProductId.HasValue
            && !await _products.AnyAsync(p => p.IdProduct == dto.ProductId.Value, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Товар с идентификатором {dto.ProductId.Value} не найден.");
        }

        var orderId = dto.OrderId ?? orderItem.OrderId;
        var productId = dto.ProductId ?? orderItem.ProductId;

        if (await _orderItems.PairExistsAsync(orderId, productId, id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "В заказе уже есть позиция с указанным товаром.");
        }

        _mapper.Map(dto, orderItem);
        _orderItems.Update(orderItem);
        await _orderItems.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var orderItem = await _orderItems.GetByIdAsync(id, cancellationToken);
        if (orderItem is null)
        {
            return ServiceResult<bool>.NotFound($"Позиция заказа с идентификатором {id} не найдена.");
        }

        _orderItems.Remove(orderItem);
        await _orderItems.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }
}
