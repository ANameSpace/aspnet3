using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.Roles;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/roles")]
[ApiController]
public class RolesApiController : ApiControllerBase
{
    private readonly IRoleService _roles;

    public RolesApiController(IRoleService roles)
    {
        _roles = roles;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _roles.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetRole(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _roles.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole(
        CreateRoleDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _roles.CreateAsync(dto, cancellationToken),
            nameof(GetRole),
            role => role.IdRole);

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateRole(
        int id,
        UpdateRoleDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _roles.UpdateAsync(id, dto, cancellationToken));

    [HttpPatch("{id}")]
    public async Task<ActionResult<bool>> PatchRole(
        int id,
        PatchRoleDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _roles.PatchAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteRole(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _roles.DeleteAsync(id, cancellationToken));
}
