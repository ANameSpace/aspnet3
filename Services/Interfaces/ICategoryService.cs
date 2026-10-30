using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.Categories;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface ICategoryService
{
    Task<ServiceResult<PagedResult<CategoryDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CategoryDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CategoryDto>> CreateAsync(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateCategoryDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchCategoryDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
