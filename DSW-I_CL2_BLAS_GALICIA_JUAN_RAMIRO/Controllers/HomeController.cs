using DSW_I_CL2_BLAS_GALICIA_JUAN_RAMIRO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DSW_I_CL2_BLAS_GALICIA_JUAN_RAMIRO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
