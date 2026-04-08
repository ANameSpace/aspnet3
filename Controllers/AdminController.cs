using Microsoft.AspNetCore.Mvc;

namespace PSite3.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}