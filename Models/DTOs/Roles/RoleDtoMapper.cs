namespace PSite3.Models.DTOs.Roles;

public static class RoleDtoMapper
{
    public static RoleDto ToDto(Role role)
    {
        return new RoleDto
        {
            IdRole = role.IdRole,
            Name = role.Name
        };
    }
}
