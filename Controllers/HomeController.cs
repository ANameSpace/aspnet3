using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using System.Diagnostics;

namespace PSite3.Controllers
{
    public class HomeController(ShopContext context) : Controller
    {
        private readonly int USER_ID = 1;

        public async Task<IActionResult> Index()
        {
            var products = await context.Products
                .Include(p => p.Category)
                .ToListAsync();

            var cart = await context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == USER_ID);

            var cartItems = new Dictionary<int, int>();
            if (cart?.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    cartItems[item.ProductId] = item.Quantity;
                }
            }

            ViewBag.CartItems = cartItems;

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
