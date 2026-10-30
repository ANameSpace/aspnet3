using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.Products;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/products")]
[ApiController]
public class ProductsApiController : ApiControllerBase
{
    private readonly IProductService _products;

    public ProductsApiController(IProductService products)
    {
        _products = products;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _products.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _products.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        CreateProductDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _products.CreateAsync(dto, cancellationToken),
            nameof(GetProduct),
            product => product.IdProduct);

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateProduct(
        int id,
        UpdateProductDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _products.UpdateAsync(id, dto, cancellationToken));

    [HttpPatch("{id}")]
    public async Task<ActionResult<bool>> PatchProduct(
        int id,
        PatchProductDto dto,
        CancellationToken cancellationToken = default)
        => FromResult(await _products.PatchAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteProduct(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _products.DeleteAsync(id, cancellationToken));
}
