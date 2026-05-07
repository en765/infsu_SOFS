using Microsoft.AspNetCore.Mvc;

namespace FitnessStudio.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}