using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Users;

public class PatchUserDto
{
    [StringLength(64, MinimumLength = 3)]
    public string? UserName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Hash { get; set; }

    [Range(0, int.MaxValue)]
    public int? RoleId { get; set; }

    public bool? IsActive { get; set; }
}
