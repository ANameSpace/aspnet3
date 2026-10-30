using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.CartItems;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface ICartItemService
{
    Task<ServiceResult<PagedResult<CartItemDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CartItemDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CartItemDto>> CreateAsync(
        CreateCartItemDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateCartItemDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchCartItemDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
