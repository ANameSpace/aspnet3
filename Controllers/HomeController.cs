using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace PSite3.Controllers
{
    public class HomeController(ShopContext _context) : Controller
    {
        private const int PageSize = 8;

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchString,
            int? categoryId,
            string? sortBy,
            int pageNumber = 1)
        {
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery
                    .Where(p => p.Name.Contains(searchString) ||
                               (p.Description != null && p.Description.Contains(searchString)));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            productsQuery = sortBy switch
            {
                "name_asc" => productsQuery.OrderBy(p => p.Name),
                "name_desc" => productsQuery.OrderByDescending(p => p.Name),
                "price_asc" => productsQuery.OrderBy(p => p.Price),
                "price_desc" => productsQuery.OrderByDescending(p => p.Price),
                _ => productsQuery.OrderBy(p => p.IdProduct)
            };

            var totalItems = await productsQuery.CountAsync();
            var products = await productsQuery
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var userId))
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                ViewBag.CartItems = cart?.CartItems?
                    .ToDictionary(i => i.ProductId, i => i.Quantity) ?? new Dictionary<int, int>();
            }

            ViewBag.SearchString = searchString;
            ViewBag.CategoryId = categoryId;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}