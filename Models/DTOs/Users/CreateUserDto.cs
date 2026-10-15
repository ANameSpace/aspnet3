using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Users;

public class CreateUserDto
{
    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Hash { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;
}
