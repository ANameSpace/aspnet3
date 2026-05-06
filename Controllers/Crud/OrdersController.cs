using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    [Authorize(Roles = "Admin")]
    public class OrdersController(ShopContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.ToListAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _context.Users.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            ModelState.Remove("User");
            ModelState.Remove("OrderItems");

            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _context.Users.ToListAsync();
                return View(order);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id.Value);
            if (order == null) return NotFound();

            ViewBag.Users = await _context.Users.ToListAsync();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.IdOrder) return BadRequest();

            ModelState.Remove("User");
            ModelState.Remove("OrderItems");

            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _context.Users.ToListAsync();
                return View(order);
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.IdOrder == id.Value);
            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}