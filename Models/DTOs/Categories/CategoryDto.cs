using System.Collections.Generic;
using PSite3.Models.DTOs.Products;

namespace PSite3.Models.DTOs.Categories;

public class CategoryDto
{
    public int IdCategory { get; set; }
    public string Name { get; set; } = null!;
    public List<ProductSummaryDto> Products { get; set; } = new();
}
