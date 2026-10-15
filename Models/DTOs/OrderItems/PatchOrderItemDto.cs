using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.OrderItems;

public class PatchOrderItemDto
{
    [Range(0, int.MaxValue)]
    public int? OrderId { get; set; }

    [Range(0, int.MaxValue)]
    public int? ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }

    [Range(0.1, double.MaxValue)]
    public decimal? Price { get; set; }
}
