using FitnessStudio.Business.Interfaces;
using FitnessStudio.Domain.Models;
using FitnessStudio.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessStudio.Web.Controllers
{
    public class ClanarinaController : Controller
    {
        private readonly IClanarinaService _clanarinaService;
        private readonly IUplataService _uplataService;
        private readonly IClanService _clanService;
        private readonly IPaketService _paketService;

        public ClanarinaController(
            IClanarinaService clanarinaService,
            IUplataService uplataService,
            IClanService clanService,
            IPaketService paketService)
        {
            _clanarinaService = clanarinaService;
            _uplataService = uplataService;
            _clanService = clanService;
            _paketService = paketService;
        }

        public IActionResult Index(string? search, int? selectedClanarinaId)
        {
            var clanarine = string.IsNullOrWhiteSpace(search)
                ? _clanarinaService.GetAll()
                : _clanarinaService.Search(search);

            var clanovi = _clanService.GetAll()
                .ToDictionary(c => c.ClanId, c => $"{c.Ime} {c.Prezime}");

            var paketi = _paketService.GetAll()
                .ToDictionary(p => p.PaketId, p => p.Naziv);

            if (!selectedClanarinaId.HasValue)
            {
                selectedClanarinaId = clanarine.FirstOrDefault()?.ClanarinaId;
            }

            var uplate = selectedClanarinaId.HasValue
                ? _uplataService.GetByClanarinaId(selectedClanarinaId.Value)
                : Enumerable.Empty<Uplata>();

            var model = new ClanarinaIndexViewModel
            {
                Clanarine = clanarine,
                Uplate = uplate,
                SelectedClanarinaId = selectedClanarinaId,
                Search = search,
                Clanovi = clanovi,
                Paketi = paketi
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ClanarinaFormViewModel
            {
                DatumPocetka = DateTime.Today,
                DatumZavrsetka = DateTime.Today.AddMonths(1),
                Status = StatusClanarine.Aktivna
            };

            PopulateDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ClanarinaFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            var clanarina = new Clanarina
            {
                ClanId = model.ClanId,
                PaketId = model.PaketId,
                DatumPocetka = model.DatumPocetka,
                DatumZavrsetka = model.DatumZavrsetka,
                Status = model.Status
            };

            if (!_clanarinaService.Create(clanarina, out var errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Došlo je do pogreške.");
                PopulateDropdowns(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var clanarina = _clanarinaService.GetById(id);
            if (clanarina is null)
            {
                return NotFound();
            }

            var model = new ClanarinaFormViewModel
            {
                ClanarinaId = clanarina.ClanarinaId,
                ClanId = clanarina.ClanId,
                PaketId = clanarina.PaketId,
                DatumPocetka = clanarina.DatumPocetka,
                DatumZavrsetka = clanarina.DatumZavrsetka,
                Status = clanarina.Status
            };

            PopulateDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ClanarinaFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            var clanarina = new Clanarina
            {
                ClanarinaId = model.ClanarinaId,
                ClanId = model.ClanId,
                PaketId = model.PaketId,
                DatumPocetka = model.DatumPocetka,
                DatumZavrsetka = model.DatumZavrsetka,
                Status = model.Status
            };

            if (!_clanarinaService.Update(clanarina, out var errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Došlo je do pogreške.");
                PopulateDropdowns(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var clanarina = _clanarinaService.GetById(id);
            if (clanarina is null)
            {
                return NotFound();
            }

            var clan = _clanService.GetById(clanarina.ClanId);
            var paket = _paketService.GetById(clanarina.PaketId);

            ViewBag.ClanNaziv = clan is null ? "Nepoznat član" : $"{clan.Ime} {clan.Prezime}";
            ViewBag.PaketNaziv = paket?.Naziv ?? "Nepoznat paket";

            return View(clanarina);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int clanarinaId)
        {
            if (!_clanarinaService.Delete(clanarinaId, out var errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage ?? "Došlo je do pogreške prilikom brisanja.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns(ClanarinaFormViewModel model)
        {
            model.Clanovi = _clanService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.ClanId.ToString(),
                    Text = $"{c.Ime} {c.Prezime}"
                })
                .ToList();

            model.Paketi = _paketService.GetAll()
                .Select(p => new SelectListItem
                {
                    Value = p.PaketId.ToString(),
                    Text = p.Naziv
                })
                .ToList();
        }
    }
}