using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.Users;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/users")]
[ApiController]
public class UsersApiController : ApiControllerBase
{
    private readonly IUserService _users;

    public UsersApiController(IUserService users)
    {
        _users = users;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _users.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _users.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(
        CreateUserDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _users.CreateAsync(dto, cancellationToken),
            nameof(GetUser),
            user => user.IdUser);

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateUser(
        int id,
        UpdateUserDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _users.UpdateAsync(id, dto, cancellationToken));

    [HttpPatch("{id}")]
    public async Task<ActionResult<bool>> PatchUser(
        int id,
        PatchUserDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _users.PatchAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteUser(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _users.DeleteAsync(id, cancellationToken));
}
