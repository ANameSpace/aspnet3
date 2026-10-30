using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.Products;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface IProductService
{
    Task<ServiceResult<PagedResult<ProductDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<ProductDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<ProductDto>> CreateAsync(
        CreateProductDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateProductDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchProductDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
