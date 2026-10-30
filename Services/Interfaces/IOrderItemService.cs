using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.OrderItems;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface IOrderItemService
{
    Task<ServiceResult<PagedResult<OrderItemDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderItemDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderItemDto>> CreateAsync(
        CreateOrderItemDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateOrderItemDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchOrderItemDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
