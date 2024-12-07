using Microsoft.AspNetCore.Mvc;

namespace DSW_I_CL2_BLAS_GALICIA_JUAN_RAMIRO.Controllers
{
    public class AdaptadorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Adaptadores()
        {
            return View();
        }
    }
}
