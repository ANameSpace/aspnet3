using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Models.DTOs.Products;

namespace PSite3.Controllers.Api;

[Route("api/products")]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly ShopContext _context;

    public ProductsApiController(ShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var products = await _context.Products
            .OrderBy(p => p.IdProduct)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(products.Select(ProductDtoMapper.ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(ProductDtoMapper.ToDto(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createDto)
    {
        var product = new Product
        {
            Name = createDto.Name,
            Description = createDto.Description,
            Price = createDto.Price,
            CategoryId = createDto.CategoryId,
            Image = createDto.Image
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = ProductDtoMapper.ToDto(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.IdProduct }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = updateDto.Name;
        product.Description = updateDto.Description;
        product.Price = updateDto.Price;
        product.CategoryId = updateDto.CategoryId;
        product.Image = updateDto.Image;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchProduct(int id, PatchProductDto patchDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        if (patchDto.Name != null) product.Name = patchDto.Name;
        if (patchDto.Description != null) product.Description = patchDto.Description;
        if (patchDto.Price.HasValue) product.Price = patchDto.Price.Value;
        if (patchDto.CategoryId.HasValue) product.CategoryId = patchDto.CategoryId.Value;
        if (patchDto.Image != null) product.Image = patchDto.Image;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
