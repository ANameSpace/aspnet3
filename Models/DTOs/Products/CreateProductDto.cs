using System.ComponentModel.DataAnnotations;
using PSite3.Models.Attribute;

namespace PSite3.Models.DTOs.Products;

public class CreateProductDto
{
    [Required]
    [StringLength(32)]
    [Name]
    public string Name { get; set; } = null!;

    [StringLength(512)]
    [Censorship]
    [Sentence]
    public string? Description { get; set; }

    [Required]
    [Range(0.1, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int CategoryId { get; set; }

    [StringLength(256)]
    public string? Image { get; set; }
}
