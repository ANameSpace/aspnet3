using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.Categories;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/categories")]
[ApiController]
public class CategoriesApiController : ApiControllerBase
{
    private readonly ICategoryService _categories;

    public CategoriesApiController(ICategoryService categories)
    {
        _categories = categories;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _categories.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _categories.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _categories.CreateAsync(dto, cancellationToken),
            nameof(GetCategory),
            category => category.IdCategory);

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateCategory(
        int id,
        UpdateCategoryDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _categories.UpdateAsync(id, dto, cancellationToken));

    [HttpPatch("{id}")]
    public async Task<ActionResult<bool>> PatchCategory(
        int id,
        PatchCategoryDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _categories.PatchAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteCategory(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _categories.DeleteAsync(id, cancellationToken));
}
