using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Carts;

public class CreateCartDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int UserId { get; set; }
}
