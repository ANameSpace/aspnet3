using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.Orders;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface IOrderService
{
    Task<ServiceResult<PagedResult<OrderDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderDto>> CreateAsync(
        CreateOrderDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateOrderDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchOrderDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
