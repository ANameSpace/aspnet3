using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    public class UsersController(ShopContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            ModelState.Remove("Role");
            ModelState.Remove("Carts");
            ModelState.Remove("Orders");

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
            if (existing != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id.Value);
            if (user == null) return NotFound();

            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.IdUser) return BadRequest();

            ModelState.Remove("Role");
            ModelState.Remove("Carts");
            ModelState.Remove("Orders");

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == user.Login && u.IdUser != user.IdUser);
            if (existing != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}