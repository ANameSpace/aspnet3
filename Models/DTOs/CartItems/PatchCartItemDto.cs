using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.CartItems;

public class PatchCartItemDto
{
    [Range(0, int.MaxValue)]
    public int? CartId { get; set; }

    [Range(0, int.MaxValue)]
    public int? ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }
}
