using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Products;

public class PatchProductDto
{
    [StringLength(32)]
    public string? Name { get; set; }

    [StringLength(512)]
    public string? Description { get; set; }

    [Range(0.1, double.MaxValue)]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue)]
    public int? CategoryId { get; set; }

    public string? Image { get; set; }
}
