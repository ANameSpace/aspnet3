using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace PSite3.Controllers
{
    public class HomeController(ShopContext _context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == 1);

            Console.WriteLine($"UserName from DB: {user?.UserName}");

            return Content($"DB: {user?.UserName}<br/>Browser encoding: UTF-8", "text/html");
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var userId))
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                ViewBag.CartItems = cart?.CartItems?
                    .ToDictionary(i => i.ProductId, i => i.Quantity) ?? [];
            }

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}