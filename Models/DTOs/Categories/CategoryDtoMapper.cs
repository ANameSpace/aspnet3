namespace PSite3.Models.DTOs.Categories;

public static class CategoryDtoMapper
{
    public static CategoryDto ToDto(Category category)
    {
        return new CategoryDto
        {
            IdCategory = category.IdCategory,
            Name = category.Name
        };
    }
}
