using System.Diagnostics;
using GameIdle.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameIdle.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // ✅ Wenn eingeloggt -> direkt ins Spiel
            if (User?.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Game");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
