using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    public class CartsController(ShopContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var carts = await _context.Carts.ToListAsync();
            return View(carts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _context.Users.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cart cart)
        {
            ModelState.Remove("User");
            ModelState.Remove("CartItems");

            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _context.Users.ToListAsync();
                return View(cart);
            }

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cart = await _context.Carts.FindAsync(id.Value);
            if (cart == null) return NotFound();

            ViewBag.Users = await _context.Users.ToListAsync();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Cart cart)
        {
            if (id != cart.IdCart) return BadRequest();

            ModelState.Remove("User");
            ModelState.Remove("CartItems");

            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _context.Users.ToListAsync();
                return View(cart);
            }

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.IdCart == id.Value);
            if (cart == null) return NotFound();

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return NotFound();

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}