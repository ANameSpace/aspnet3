using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.Categories;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categories;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categories, IMapper mapper)
    {
        _categories = categories;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<CategoryDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _categories.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<CategoryDto>>.Ok(new PagedResult<CategoryDto>
        {
            Items = _mapper.Map<List<CategoryDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<CategoryDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category = await _categories.GetWithDetailsAsync(id, cancellationToken);

        return category is null
            ? ServiceResult<CategoryDto>.NotFound($"Категория с идентификатором {id} не найдена.")
            : ServiceResult<CategoryDto>.Ok(_mapper.Map<CategoryDto>(category));
    }

    public async Task<ServiceResult<CategoryDto>> CreateAsync(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var category = _mapper.Map<Category>(dto);

        await _categories.AddAsync(category, cancellationToken);
        await _categories.SaveChangesAsync(cancellationToken);

        var created = await _categories.GetWithDetailsAsync(category.IdCategory, cancellationToken)
                      ?? category;

        return ServiceResult<CategoryDto>.Created(_mapper.Map<CategoryDto>(created));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return ServiceResult<bool>.NotFound($"Категория с идентификатором {id} не найдена.");
        }

        _mapper.Map(dto, category);
        _categories.Update(category);
        await _categories.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return ServiceResult<bool>.NotFound($"Категория с идентификатором {id} не найдена.");
        }

        _mapper.Map(dto, category);
        _categories.Update(category);
        await _categories.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return ServiceResult<bool>.NotFound($"Категория с идентификатором {id} не найдена.");
        }

        if (await _categories.HasDependenciesAsync(id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "Невозможно удалить категорию: в ней есть товары.");
        }

        _categories.Remove(category);
        await _categories.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }
}
