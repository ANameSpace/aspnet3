using System;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Users;

public class UpdateUserDto
{
    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [StringLength(128)]
    public string? Hash { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;
}
