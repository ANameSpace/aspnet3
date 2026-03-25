using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Models.ViewModels;

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

            var viewModel = new CartViewModel
            {
                CartId = cart.IdCart,
                Items = [.. cartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.IdCartItem,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    CategoryName = ci.Product.Category?.Name ?? "Без категории"
                })],
                Total = cartItems.Sum(ci => ci.Product.Price * ci.Quantity)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var userId = USER_ID;
            var cart = await GetOrCreateCartAsync(userId);

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (item == null)
            {
                _context.CartItems.Add(new CartItem { ProductId = productId, CartId = cart.IdCart, Quantity = 1 });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int productId, int quantity)
        {
            var userId = USER_ID;
            var cart = await GetOrCreateCartAsync(userId);

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (item == null)
            {
                _context.CartItems.Add(new CartItem { ProductId = productId, CartId = cart.IdCart, Quantity = quantity });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Cart");
            }
            

            if (quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Cart");
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
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
