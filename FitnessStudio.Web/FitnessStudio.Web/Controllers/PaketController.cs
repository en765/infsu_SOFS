using FitnessStudio.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessStudio.Web.Controllers
{
    public class PaketController : Controller
    {
        private readonly IPaketService _paketService;

        public PaketController(IPaketService paketService)
        {
            _paketService = paketService;
        }

        public IActionResult Index(string? search)
        {
            var paketi = string.IsNullOrWhiteSpace(search)
                ? _paketService.GetAll()
                : _paketService.SearchByName(search);

            ViewBag.Search = search;
            return View(paketi);
        }
    }
}