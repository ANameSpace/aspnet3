using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Products;

public class UpdateProductDto
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; } = null!;

    [StringLength(512)]
    public string? Description { get; set; }

    [Required]
    [Range(0.1, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int CategoryId { get; set; }

    public string? Image { get; set; }
}
