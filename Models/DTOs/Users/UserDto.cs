namespace PSite3.Models.DTOs.Users;

public class UserDto
{
    public int IdUser { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Hash { get; set; } = null!;
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
}
