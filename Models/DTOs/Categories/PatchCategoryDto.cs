using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Categories;

public class PatchCategoryDto
{
    [StringLength(32)]
    public string? Name { get; set; }
}
