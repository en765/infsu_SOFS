using FitnessStudio.Business.Interfaces;
using FitnessStudio.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using FitnessStudio.Web.ViewModels;

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
            ViewBag.PackageNames = _paketService.GetAll().Select(p => p.Naziv).Distinct().OrderBy(n => n).ToList();
            return View(paketi);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PaketFormViewModel());
        }

        [HttpPost]
        public IActionResult Create(PaketFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!TryParseCijena(model.Cijena, out var parsedCijena))
            {
                ModelState.AddModelError(nameof(model.Cijena), "Cijena mora biti broj.");
                return View(model);
            }

            var paket = new Paket
            {
                Naziv = model.Naziv,
                BrojTreninga = model.BrojTreninga,
                Cijena = parsedCijena
            };

            if (!_paketService.Create(paket, out var errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Došlo je do pogreške.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var paket = _paketService.GetById(id);
            if (paket is null)
            {
                return NotFound();
            }

            var model = new PaketFormViewModel
            {
                PaketId = paket.PaketId,
                Naziv = paket.Naziv,
                BrojTreninga = paket.BrojTreninga,
                Cijena = paket.Cijena.ToString("0.00", CultureInfo.InvariantCulture)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PaketFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!TryParseCijena(model.Cijena, out var parsedCijena))
            {
                ModelState.AddModelError(nameof(model.Cijena), "Cijena mora biti broj.");
                return View(model);
            }

            var paket = new Paket
            {
                PaketId = model.PaketId,
                Naziv = model.Naziv,
                BrojTreninga = model.BrojTreninga,
                Cijena = parsedCijena
            };

            if (!_paketService.Update(paket, out var errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Došlo je do pogreške.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var paket = _paketService.GetById(id);
            if (paket is null)
            {
                return NotFound();
            }

            return View(paket);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int paketId)
        {
            if (!_paketService.Delete(paketId, out var errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage ?? "Došlo je do pogreške prilikom brisanja.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        private bool TryParseCijena(string input, out decimal cijena)
        {
            input = input.Trim();

            return decimal.TryParse(input.Replace('.', ','), NumberStyles.Number, new CultureInfo("hr-HR"), out cijena)
                || decimal.TryParse(input.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out cijena);
        }
    }
    
}