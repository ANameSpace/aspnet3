namespace PSite3.Models.DTOs.Users;

public class UserSummaryDto
{
    public int IdUser { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsActive { get; set; }
}
