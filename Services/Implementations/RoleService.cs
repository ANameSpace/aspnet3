using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.Roles;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roles;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roles, IMapper mapper)
    {
        _roles = roles;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<RoleDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _roles.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<RoleDto>>.Ok(new PagedResult<RoleDto>
        {
            Items = _mapper.Map<List<RoleDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<RoleDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetWithDetailsAsync(id, cancellationToken);

        return role is null
            ? ServiceResult<RoleDto>.NotFound($"Роль с идентификатором {id} не найдена.")
            : ServiceResult<RoleDto>.Ok(_mapper.Map<RoleDto>(role));
    }

    public async Task<ServiceResult<RoleDto>> CreateAsync(
        CreateRoleDto dto,
        CancellationToken cancellationToken = default)
    {
        if (await _roles.NameExistsAsync(dto.Name, null, cancellationToken))
        {
            return ServiceResult<RoleDto>.Conflict($"Роль с названием «{dto.Name}» уже существует.");
        }

        var role = _mapper.Map<Role>(dto);

        await _roles.AddAsync(role, cancellationToken);
        await _roles.SaveChangesAsync(cancellationToken);

        var created = await _roles.GetWithDetailsAsync(role.IdRole, cancellationToken)
                      ?? role;

        return ServiceResult<RoleDto>.Created(_mapper.Map<RoleDto>(created));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateRoleDto dto,
        CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken);
        if (role is null)
        {
            return ServiceResult<bool>.NotFound($"Роль с идентификатором {id} не найдена.");
        }

        if (await _roles.NameExistsAsync(dto.Name, id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict($"Роль с названием «{dto.Name}» уже существует.");
        }

        _mapper.Map(dto, role);
        _roles.Update(role);
        await _roles.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchRoleDto dto,
        CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken);
        if (role is null)
        {
            return ServiceResult<bool>.NotFound($"Роль с идентификатором {id} не найдена.");
        }

        if (dto.Name is not null
            && await _roles.NameExistsAsync(dto.Name, id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict($"Роль с названием «{dto.Name}» уже существует.");
        }

        _mapper.Map(dto, role);
        _roles.Update(role);
        await _roles.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken);
        if (role is null)
        {
            return ServiceResult<bool>.NotFound($"Роль с идентификатором {id} не найдена.");
        }

        if (await _roles.HasDependenciesAsync(id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "Невозможно удалить роль: она назначена пользователям.");
        }

        _roles.Remove(role);
        await _roles.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }
}
