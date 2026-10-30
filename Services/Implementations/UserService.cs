using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.Users;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly IRoleRepository _roles;
    private readonly IMapper _mapper;

    public UserService(IUserRepository users, IRoleRepository roles, IMapper mapper)
    {
        _users = users;
        _roles = roles;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<UserDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _users.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<UserDto>>.Ok(new PagedResult<UserDto>
        {
            Items = _mapper.Map<List<UserDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<UserDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var user = await _users.GetWithDetailsAsync(id, cancellationToken);

        return user is null
            ? ServiceResult<UserDto>.NotFound($"Пользователь с идентификатором {id} не найден.")
            : ServiceResult<UserDto>.Ok(_mapper.Map<UserDto>(user));
    }

    public async Task<ServiceResult<UserDto>> CreateAsync(
        CreateUserDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!await _roles.AnyAsync(r => r.IdRole == dto.RoleId, cancellationToken))
        {
            return ServiceResult<UserDto>.Validation(
                $"Роль с идентификатором {dto.RoleId} не найдена.");
        }

        if (await _users.UserNameExistsAsync(dto.UserName, null, cancellationToken))
        {
            return ServiceResult<UserDto>.Conflict(
                $"Пользователь с именем «{dto.UserName}» уже существует.");
        }

        var user = _mapper.Map<User>(dto);

        await _users.AddAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        var created = await _users.GetWithDetailsAsync(user.IdUser, cancellationToken)
                      ?? user;

        return ServiceResult<UserDto>.Created(_mapper.Map<UserDto>(created));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateUserDto dto,
        CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return ServiceResult<bool>.NotFound($"Пользователь с идентификатором {id} не найден.");
        }

        if (!await _roles.AnyAsync(r => r.IdRole == dto.RoleId, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Роль с идентификатором {dto.RoleId} не найдена.");
        }

        if (await _users.UserNameExistsAsync(dto.UserName, id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                $"Пользователь с именем «{dto.UserName}» уже существует.");
        }

        _mapper.Map(dto, user);
        _users.Update(user);
        await _users.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchUserDto dto,
        CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return ServiceResult<bool>.NotFound($"Пользователь с идентификатором {id} не найден.");
        }

        if (dto.RoleId.HasValue
            && !await _roles.AnyAsync(r => r.IdRole == dto.RoleId.Value, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Роль с идентификатором {dto.RoleId.Value} не найдена.");
        }

        if (dto.UserName is not null
            && await _users.UserNameExistsAsync(dto.UserName, id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                $"Пользователь с именем «{dto.UserName}» уже существует.");
        }

        _mapper.Map(dto, user);
        _users.Update(user);
        await _users.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return ServiceResult<bool>.NotFound($"Пользователь с идентификатором {id} не найден.");
        }

        if (await _users.HasDependenciesAsync(id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "Невозможно удалить пользователя: у него есть корзины, заказы или резервирования.");
        }

        _users.Remove(user);
        await _users.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }
}
