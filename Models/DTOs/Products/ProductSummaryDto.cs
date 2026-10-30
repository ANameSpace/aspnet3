namespace PSite3.Models.DTOs.Products;

public class ProductSummaryDto
{
    public int IdProduct { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public string? Image { get; set; }
}
