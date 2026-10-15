using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Models.DTOs.Users;

namespace PSite3.Controllers.Api;

[Route("api/users")]
[ApiController]
public class UsersApiController : ControllerBase
{
    private readonly ShopContext _context;

    public UsersApiController(ShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var users = await _context.Users
            .OrderBy(u => u.IdUser)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(users.Select(UserDtoMapper.ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(UserDtoMapper.ToDto(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createDto)
    {
        var user = new User
        {
            UserName = createDto.UserName,
            Email = createDto.Email,
            Hash = createDto.Hash,
            RoleId = createDto.RoleId,
            IsActive = createDto.IsActive
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var dto = UserDtoMapper.ToDto(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.IdUser }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.UserName = updateDto.UserName;
        user.Email = updateDto.Email;
        user.Hash = updateDto.Hash;
        user.RoleId = updateDto.RoleId;
        user.IsActive = updateDto.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchUser(int id, PatchUserDto patchDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (patchDto.UserName != null) user.UserName = patchDto.UserName;
        if (patchDto.Email != null) user.Email = patchDto.Email;
        if (patchDto.Hash != null) user.Hash = patchDto.Hash;
        if (patchDto.RoleId.HasValue) user.RoleId = patchDto.RoleId.Value;
        if (patchDto.IsActive.HasValue) user.IsActive = patchDto.IsActive.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
