using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Models.DTOs.Reservations;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservations;
    private readonly IProductRepository _products;
    private readonly IUserRepository _users;
    private readonly IOrderRepository _orders;
    private readonly IMapper _mapper;

    public ReservationService(
        IReservationRepository reservations,
        IProductRepository products,
        IUserRepository users,
        IOrderRepository orders,
        IMapper mapper)
    {
        _reservations = reservations;
        _products = products;
        _users = users;
        _orders = orders;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<ReservationDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _reservations.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<ReservationDto>>.Ok(new PagedResult<ReservationDto>
        {
            Items = _mapper.Map<List<ReservationDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<ReservationDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var reservation = await _reservations.GetWithDetailsAsync(id, cancellationToken);

        return reservation is null
            ? ServiceResult<ReservationDto>.NotFound($"Резервирование с идентификатором {id} не найдено.")
            : ServiceResult<ReservationDto>.Ok(_mapper.Map<ReservationDto>(reservation));
    }

    public async Task<ServiceResult<ReservationDto>> CreateAsync(
        CreateReservationDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(dto.ProductId, cancellationToken);
        if (product is null)
        {
            return ServiceResult<ReservationDto>.Validation(
                $"Товар с идентификатором {dto.ProductId} не найден.");
        }

        if (!await _users.AnyAsync(u => u.IdUser == dto.UserId, cancellationToken))
        {
            return ServiceResult<ReservationDto>.Validation(
                $"Пользователь с идентификатором {dto.UserId} не найден.");
        }

        if (dto.OrderId.HasValue
            && !await _orders.AnyAsync(o => o.IdOrder == dto.OrderId.Value, cancellationToken))
        {
            return ServiceResult<ReservationDto>.Validation(
                $"Заказ с идентификатором {dto.OrderId.Value} не найден.");
        }

        if (product.StockQuantity < dto.Quantity)
        {
            return ServiceResult<ReservationDto>.Conflict(
                $"Недостаточно товара на складе: доступно {product.StockQuantity}, запрошено {dto.Quantity}.");
        }

        product.StockQuantity -= dto.Quantity;

        var reservation = _mapper.Map<ProductReservation>(dto);
        reservation.ReservedAt = DateTime.UtcNow;
        reservation.Status = ReservationStatus.Active;

        await _reservations.AddAsync(reservation, cancellationToken);

        try
        {
            await _reservations.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return ServiceResult<ReservationDto>.Conflict(
                "Остаток товара был изменён другим запросом. Повторите резервирование.");
        }

        var created = await _reservations.GetWithDetailsAsync(reservation.IdReservation, cancellationToken)
                      ?? reservation;

        return ServiceResult<ReservationDto>.Created(_mapper.Map<ReservationDto>(created));
    }

    public async Task<ServiceResult<ReservationDto>> CancelAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var reservation = await _reservations.GetActiveWithDetailsAsync(id, cancellationToken);
        if (reservation is null)
        {
            return ServiceResult<ReservationDto>.NotFound(
                $"Активное резервирование с идентификатором {id} не найдено.");
        }

        var product = reservation.Product
                      ?? await _products.GetByIdAsync(reservation.ProductId, cancellationToken);

        if (product is null)
        {
            return ServiceResult<ReservationDto>.Conflict(
                "Невозможно отменить резервирование: товар не найден.");
        }

        product.StockQuantity += reservation.Quantity;
        reservation.Status = ReservationStatus.Cancelled;
        reservation.ClosedAt = DateTime.UtcNow;

        try
        {
            await _reservations.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return ServiceResult<ReservationDto>.Conflict(
                "Остаток товара был изменён другим запросом. Повторите отмену резервирования.");
        }

        return ServiceResult<ReservationDto>.Ok(_mapper.Map<ReservationDto>(reservation));
    }
}
