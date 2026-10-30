using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.Users;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<PagedResult<UserDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<UserDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<UserDto>> CreateAsync(
        CreateUserDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateUserDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchUserDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
