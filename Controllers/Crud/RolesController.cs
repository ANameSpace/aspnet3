using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;

namespace PSite3.Controllers.Crud
{
    public class RolesController(ShopContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            ModelState.Remove("Users");

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var existing = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role.Name);
            if (existing != null)
            {
                ModelState.AddModelError("Name", "Роль с таким названием уже существует");
                return View(role);
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var role = await _context.Roles.FindAsync(id.Value);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Role role)
        {
            if (id != role.IdRole) return BadRequest();

            ModelState.Remove("Users");

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var existing = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == role.Name && r.IdRole != role.IdRole);
            if (existing != null)
            {
                ModelState.AddModelError("Name", "Роль с таким названием уже существует");
                return View(role);
            }

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.IdRole == id.Value);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}