using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.Carts;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface ICartService
{
    Task<ServiceResult<PagedResult<CartDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CartDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CartDto>> CreateAsync(
        CreateCartDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateCartDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchCartDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
