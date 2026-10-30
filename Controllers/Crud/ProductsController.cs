using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ShopContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string _imagesPath;
        private const string _defaultImage = "no-image.jpg";

        public ProductsController(ShopContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _imagesPath = Path.Combine(_env.WebRootPath, "images", "products");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Product product, IFormFile? imageFile)
        {
            ModelState.Remove("Category");
            ModelState.Remove("CartItems");
            ModelState.Remove("OrderItems");

            if (imageFile != null && imageFile.Length > 0)
            {
                var result = await SaveImageAsync(imageFile);
                if (result.IsSuccess)
                {
                    product.Image = result.FileName;
                }
                else
                {
                    ModelState.AddModelError("imageFile", result.Error);
                }
            }
            else
            {
                product.Image = _defaultImage;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(product);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id.Value);
            if (product == null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
        {
            if (id != product.IdProduct) return BadRequest();

            ModelState.Remove("Category");
            ModelState.Remove("CartItems");
            ModelState.Remove("OrderItems");

            var existingProduct = await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProduct == id);

            if (existingProduct == null) return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingProduct.Image) &&
                    existingProduct.Image != _defaultImage)
                {
                    DeleteImage(existingProduct.Image);
                }

                var result = await SaveImageAsync(imageFile);
                if (result.IsSuccess)
                {
                    product.Image = result.FileName;
                }
                else
                {
                    ModelState.AddModelError("imageFile", result.Error);
                }
            }
            else
            {
                product.Image = existingProduct.Image ?? _defaultImage;
            }

            // Остаток управляется API резервирования; форма MVC его не содержит.
            product.StockQuantity = existingProduct.StockQuantity;

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(product);
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.IdProduct == id.Value);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.Image) &&
                product.Image != _defaultImage)
            {
                DeleteImage(product.Image);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<(bool IsSuccess, string? FileName, string? Error)> SaveImageAsync(IFormFile file)
        {
            try
            {
                if (file.Length > 5 * 1024 * 1024)
                    return (false, null, "Размер файла не должен превышать 5 МБ");

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                if (!allowedExts.Contains(ext))
                    return (false, null, "Допустимы только изображения: JPG, PNG, GIF, WEBP");

                var fileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(_imagesPath, fileName);

                Directory.CreateDirectory(_imagesPath);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return (true, fileName, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Ошибка сохранения: {ex.Message}");
            }
        }

        private void DeleteImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName == _defaultImage)
                return;

            try
            {
                var filePath = Path.Combine(_imagesPath, fileName);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            catch
            {
                //_logger.LogWarning("Не удалось удалить файл: {FileName}", fileName);
            }
        }
    }
}