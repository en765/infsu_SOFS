using FitnessStudio.Business.Interfaces;
using FitnessStudio.Domain.Models;
using FitnessStudio.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization; 

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

            string? odabraniPaketNaziv = null;
            decimal? odabraniPaketCijena = null;
            decimal ukupnoUplaceno = 0;
            decimal preostaloZaUplatu = 0;

            var odabranaClanarina = selectedClanarinaId.HasValue
                ? clanarine.FirstOrDefault(c => c.ClanarinaId == selectedClanarinaId.Value)
                : null;

            if (odabranaClanarina is not null)
            {
                var paket = _paketService.GetById(odabranaClanarina.PaketId);

                if (paket is not null)
                {
                    odabraniPaketNaziv = paket.Naziv;
                    odabraniPaketCijena = paket.Cijena;
                    ukupnoUplaceno = uplate.Sum(u => u.Iznos);
                    preostaloZaUplatu = paket.Cijena - ukupnoUplaceno;
                }
            }

            var model = new ClanarinaIndexViewModel
            {
                Clanarine = clanarine,
                Uplate = uplate,
                SelectedClanarinaId = selectedClanarinaId,
                Search = search,
                Clanovi = clanovi,
                Paketi = paketi,
                OdabraniPaketNaziv = odabraniPaketNaziv,
                OdabraniPaketCijena = odabraniPaketCijena,
                UkupnoUplaceno = ukupnoUplaceno,
                PreostaloZaUplatu = preostaloZaUplatu
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
        private bool TryParseDecimalInput(string input, out decimal value)
        {
            input = input.Trim();

            return decimal.TryParse(input.Replace('.', ','), NumberStyles.Number, new CultureInfo("hr-HR"), out value)
                || decimal.TryParse(input.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        [HttpGet]
        public IActionResult CreateUplata(int clanarinaId)
        {
            var clanarina = _clanarinaService.GetById(clanarinaId);
            if (clanarina is null)
            {
                return NotFound();
            }

            var model = new UplataFormViewModel
            {
                ClanarinaId = clanarinaId,
                Datum = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateUplata(UplataFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!TryParseDecimalInput(model.Iznos, out var parsedIznos))
            {
                ModelState.AddModelError(nameof(model.Iznos), "Iznos mora biti broj.");
                return View(model);
            }

            var uplata = new Uplata
            {
                ClanarinaId = model.ClanarinaId,
                Iznos = parsedIznos,
                Datum = model.Datum
            };

            if (!_uplataService.Create(uplata, out var errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Došlo je do pogreške.");
                return View(model);
            }

            return RedirectToAction(nameof(Index), new { selectedClanarinaId = model.ClanarinaId });
        }

        [HttpGet]
        public IActionResult EditUplata(int id)
        {
            var uplata = _uplataService.GetById(id);
            if (uplata is null)
            {
                return NotFound();
            }

            var model = new UplataFormViewModel
            {
                UplataId = uplata.UplataId,
                ClanarinaId = uplata.ClanarinaId,
                Iznos = uplata.Iznos.ToString("0.00", CultureInfo.InvariantCulture),
                Datum = uplata.Datum
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditUplata(UplataFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!TryParseDecimalInput(model.Iznos, out var parsedIznos))
            {
                ModelState.AddModelError(nameof(model.Iznos), "Iznos mora biti broj.");
                return View(model);
            }

            var uplata = new Uplata
            {
                UplataId = model.UplataId,
                ClanarinaId = model.ClanarinaId,
                Iznos = parsedIznos,
                Datum = model.Datum
            };

            if (!_uplataService.Update(uplata, out var errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Došlo je do pogreške.");
                return View(model);
            }

            return RedirectToAction(nameof(Index), new { selectedClanarinaId = model.ClanarinaId });
        }

        [HttpGet]
        public IActionResult DeleteUplata(int id)
        {
            var uplata = _uplataService.GetById(id);
            if (uplata is null)
            {
                return NotFound();
            }

            return View(uplata);
        }

        [HttpPost, ActionName("DeleteUplata")]
        public IActionResult DeleteUplataConfirmed(int uplataId, int clanarinaId)
        {
            if (!_uplataService.Delete(uplataId, out var errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage ?? "Došlo je do pogreške prilikom brisanja uplate.";
            }

            return RedirectToAction(nameof(Index), new { selectedClanarinaId = clanarinaId });
        }
    }
}