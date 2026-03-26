using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers
{
    public class CartController(ShopContext _context) : Controller
    {
        private readonly int USER_ID = 1;
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = USER_ID;

            var cart = await GetOrCreateCartAsync(userId);
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cart.IdCart)
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Category)
                .ToListAsync();
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int productId, int quantity)
        {
            var cart = await GetOrCreateCartAsync(USER_ID);
            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (item == null)
            {
                if (quantity > 0)
                {
                    _context.CartItems.Add(new CartItem { ProductId = productId, CartId = cart.IdCart, Quantity = quantity });
                }
                else
                {
                    return BadRequest();
                }
            }
            else if (quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }  
            else
            {
                item.Quantity = quantity;
            }
                

            await _context.SaveChangesAsync();

            var referer = Request.Headers.Referer.ToString();
            return !string.IsNullOrEmpty(referer) ? Redirect(referer) : RedirectToAction("Index", "Cart");
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                _context.Carts.Add(cart);

                await _context.SaveChangesAsync();

                cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            return cart!;
        }

                                                                
    }
}
