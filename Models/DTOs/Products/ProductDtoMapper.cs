namespace PSite3.Models.DTOs.Products;

public static class ProductDtoMapper
{
    public static ProductDto ToDto(Product book)
    {
        return new ProductDto
        {
            IdProduct = book.IdProduct,
            Name = book.Name,
            Description = book.Description,
            Price = book.Price,
            CategoryId = book.CategoryId,
            Image = book.Image
        };
    }
}
