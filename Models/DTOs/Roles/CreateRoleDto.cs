using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Roles;

public class CreateRoleDto
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; } = null!;
}
