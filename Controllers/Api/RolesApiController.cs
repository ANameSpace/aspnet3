using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Models.DTOs.Roles;

namespace PSite3.Controllers.Api;

[Route("api/roles")]
[ApiController]
public class RolesApiController : ControllerBase
{
    private readonly ShopContext _context;

    public RolesApiController(ShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var roles = await _context.Roles
            .OrderBy(r => r.IdRole)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(roles.Select(RoleDtoMapper.ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        return Ok(RoleDtoMapper.ToDto(role));
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleDto createDto)
    {
        var role = new Role
        {
            Name = createDto.Name
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        var dto = RoleDtoMapper.ToDto(role);
        return CreatedAtAction(nameof(GetRole), new { id = role.IdRole }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto updateDto)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        role.Name = updateDto.Name;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchRole(int id, PatchRoleDto patchDto)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        if (patchDto.Name != null) role.Name = patchDto.Name;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
