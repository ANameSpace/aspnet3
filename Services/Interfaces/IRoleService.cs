using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.Roles;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface IRoleService
{
    Task<ServiceResult<PagedResult<RoleDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<RoleDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<RoleDto>> CreateAsync(
        CreateRoleDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateRoleDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchRoleDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
