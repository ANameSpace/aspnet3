using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    public class CategoriesController(ShopContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            ModelState.Remove("Products");

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var existing = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category.Name);
            if (existing != null)
            {
                ModelState.AddModelError("Name", "Категория с таким названием уже существует");
                return View(category);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id.Value);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.IdCategory) return BadRequest();

            ModelState.Remove("Products");

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var existing = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == category.Name && c.IdCategory != category.IdCategory);
            if (existing != null)
            {
                ModelState.AddModelError("Name", "Категория с таким названием уже существует");
                return View(category);
            }

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.IdCategory == id.Value);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}