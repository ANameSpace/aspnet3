using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    [Authorize(Roles = "Admin")]
    public class OrderItemsController(ShopContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var orderItems = await _context.OrderItems.ToListAsync();
            return View(orderItems);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Orders = await _context.Orders.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderItem item)
        {
            ModelState.Remove("Order");
            ModelState.Remove("Product");
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _context.Orders.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(item);
            }

            var existingItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == item.OrderId && oi.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                existingItem.Price = item.Price;
                _context.OrderItems.Update(existingItem);
            }
            else
            {
                _context.OrderItems.Add(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.OrderItems.FindAsync(id.Value);
            if (item == null) return NotFound();

            ViewBag.Orders = await _context.Orders.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderItem item)
        {
            if (id != item.IdCartItem) return BadRequest();

            ModelState.Remove("Order");
            ModelState.Remove("Product");
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _context.Orders.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(item);
            }

            var existingItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == item.OrderId && oi.ProductId == item.ProductId && oi.IdCartItem != item.IdCartItem);

            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
                existingItem.Price = item.Price;
                _context.OrderItems.Update(existingItem);
                _context.OrderItems.Remove(item);
            }
            else
            {
                _context.OrderItems.Update(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.OrderItems.FirstOrDefaultAsync(oi => oi.IdCartItem == id.Value);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}