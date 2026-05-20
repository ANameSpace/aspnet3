using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using System.Security.Claims;

namespace PSite3.Controllers
{
    [Authorize]
    public class ShopCartController(ShopContext _context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var cart = await GetOrCreateCartAsync(userId.Value);

            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cart.IdCart)
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                .OrderByDescending(ci => ci.IdCartItem)
                .ToListAsync();

            ViewBag.TotalAmount = cartItems.Sum(i => i.Product.Price * i.Quantity);
            ViewBag.TotalItems = cartItems.Sum(i => i.Quantity);

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int productId, int quantity)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var cart = await GetOrCreateCartAsync(userId.Value);
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.IdCart && ci.ProductId == productId);

            if (item == null)
            {
                if (quantity > 0)
                {
                    var product = await _context.Products.FindAsync(productId);
                    if (product == null) return NotFound();

                    _context.CartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        CartId = cart.IdCart,
                        Quantity = quantity
                    });
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
            return !string.IsNullOrEmpty(referer) && referer.Contains("Home")
                ? Redirect(referer)
                : RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart?.CartItems != null && cart.CartItems.Any())
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            return cart!;
        }

        private int? GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idClaim, out var id) ? id : null;
        }


        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart?.CartItems == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.UtcNow,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product?.Price ?? 0
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}