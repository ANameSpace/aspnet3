using System.Collections.Generic;
using PSite3.Models.DTOs.Users;

namespace PSite3.Models.DTOs.Roles;

public class RoleDto
{
    public int IdRole { get; set; }
    public string Name { get; set; } = null!;
    public List<UserSummaryDto> Users { get; set; } = new();
}
