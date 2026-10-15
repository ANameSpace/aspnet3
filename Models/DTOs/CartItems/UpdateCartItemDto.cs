using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.CartItems;

public class UpdateCartItemDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int CartId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
