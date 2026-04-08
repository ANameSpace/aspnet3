using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    public class CartItemsController(ShopContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            return View(cartItems);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Carts = await _context.Carts.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CartItem item)
        {
            ModelState.Remove("Cart");
            ModelState.Remove("Product");
            if (!ModelState.IsValid)
            {
                ViewBag.Carts = await _context.Carts.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(item);
            }

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == item.CartId && ci.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                _context.CartItems.Add(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var items = await _context.CartItems.FindAsync(id.Value);
            if (items == null) return NotFound();

            ViewBag.Carts = await _context.Carts.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CartItem item)
        {
            if (id != item.IdCartItem) return BadRequest();

            ModelState.Remove("Cart");
            ModelState.Remove("Product");
            if (!ModelState.IsValid)
            {
                ViewBag.Carts = await _context.Carts.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(item);
            }

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == item.CartId && ci.ProductId == item.ProductId && ci.IdCartItem != item.IdCartItem);

            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
                _context.CartItems.Update(existingItem);
                _context.CartItems.Remove(item);
            } else
            {
                _context.CartItems.Update(item);
            }
  
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.CartItems.FirstOrDefaultAsync(p => p.IdCartItem == id.Value);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
