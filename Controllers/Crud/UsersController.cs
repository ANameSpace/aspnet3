using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    [Authorize(Roles = "Admin")]
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
            ModelState.Remove("IsActive");

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            var existingLogin = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingLogin != null)
            {
                ModelState.AddModelError("UserName", "Пользователь с таким именем уже существует");
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Пользователь с такой почтой уже существует");
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            user.Hash = BCrypt.Net.BCrypt.HashPassword(user.Hash);
            user.IsActive = true;


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
            ModelState.Remove("IsActive");

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            var existingLogin = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == user.UserName && u.IdUser != user.IdUser);
            var existingEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email && u.IdUser != user.IdUser);

            if (existingLogin != null)
            {
                ModelState.AddModelError("UserName", "Пользователь с таким именем уже существует");
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Пользователь с такой почтой уже существует");
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }

            if (!user.Hash.StartsWith("$"))
            {
                user.Hash = BCrypt.Net.BCrypt.HashPassword(user.Hash);
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