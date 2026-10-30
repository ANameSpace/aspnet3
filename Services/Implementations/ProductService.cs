using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.Products;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Common;
using PSite3.Services.Interfaces;

namespace PSite3.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository products,
        ICategoryRepository categories,
        IMapper mapper)
    {
        _products = products;
        _categories = categories;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PagedResult<ProductDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        (page, pageSize) = Pagination.Normalize(page, pageSize);

        var (items, totalCount) = await _products.GetPagedWithDetailsAsync(page, pageSize, cancellationToken);

        return ServiceResult<PagedResult<ProductDto>>.Ok(new PagedResult<ProductDto>
        {
            Items = _mapper.Map<List<ProductDto>>(items),
            TotalCount = totalCount
        });
    }

    public async Task<ServiceResult<ProductDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product = await _products.GetWithDetailsAsync(id, cancellationToken);

        return product is null
            ? ServiceResult<ProductDto>.NotFound($"Товар с идентификатором {id} не найден.")
            : ServiceResult<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
    }

    public async Task<ServiceResult<ProductDto>> CreateAsync(
        CreateProductDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!await _categories.AnyAsync(c => c.IdCategory == dto.CategoryId, cancellationToken))
        {
            return ServiceResult<ProductDto>.Validation(
                $"Категория с идентификатором {dto.CategoryId} не найдена.");
        }

        var product = _mapper.Map<Product>(dto);

        await _products.AddAsync(product, cancellationToken);
        await _products.SaveChangesAsync(cancellationToken);

        var created = await _products.GetWithDetailsAsync(product.IdProduct, cancellationToken)
                      ?? product;

        return ServiceResult<ProductDto>.Created(_mapper.Map<ProductDto>(created));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(
        int id,
        UpdateProductDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return ServiceResult<bool>.NotFound($"Товар с идентификатором {id} не найден.");
        }

        if (!await _categories.AnyAsync(c => c.IdCategory == dto.CategoryId, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Категория с идентификатором {dto.CategoryId} не найдена.");
        }

        _mapper.Map(dto, product);
        _products.Update(product);
        await _products.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> PatchAsync(
        int id,
        PatchProductDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return ServiceResult<bool>.NotFound($"Товар с идентификатором {id} не найден.");
        }

        if (dto.CategoryId.HasValue
            && !await _categories.AnyAsync(c => c.IdCategory == dto.CategoryId.Value, cancellationToken))
        {
            return ServiceResult<bool>.Validation(
                $"Категория с идентификатором {dto.CategoryId.Value} не найдена.");
        }

        _mapper.Map(dto, product);
        _products.Update(product);
        await _products.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }

    public async Task<ServiceResult<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return ServiceResult<bool>.NotFound($"Товар с идентификатором {id} не найден.");
        }

        if (await _products.HasDependenciesAsync(id, cancellationToken))
        {
            return ServiceResult<bool>.Conflict(
                "Невозможно удалить товар: на него ссылаются позиции корзины, заказа или резервирования.");
        }

        _products.Remove(product);
        await _products.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.NoContent();
    }
}
