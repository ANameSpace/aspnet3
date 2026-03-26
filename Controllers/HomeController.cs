using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using System.Diagnostics;

namespace PSite3.Controllers
{
    public class HomeController(ShopContext _context) : Controller
    {
        private readonly int USER_ID = 1;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = USER_ID;

            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            if (userId != 0) 
            {
                var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == USER_ID);

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
