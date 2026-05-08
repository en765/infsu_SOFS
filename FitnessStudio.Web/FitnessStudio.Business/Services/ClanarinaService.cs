using FitnessStudio.Business.Interfaces;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Business.Services
{
    public class ClanarinaService : IClanarinaService
    {
        private readonly IClanarinaRepository _clanarinaRepository;
        private readonly IClanRepository _clanRepository;
        private readonly IPaketRepository _paketRepository;
        private readonly IUplataRepository _uplataRepository;

        public ClanarinaService(
            IClanarinaRepository clanarinaRepository,
            IClanRepository clanRepository,
            IPaketRepository paketRepository,
            IUplataRepository uplataRepository)
        {
            _clanarinaRepository = clanarinaRepository;
            _clanRepository = clanRepository;
            _paketRepository = paketRepository;
            _uplataRepository = uplataRepository;
        }

        public IEnumerable<Clanarina> GetAll()
        {
            return _clanarinaRepository.GetAll();
        }

        public IEnumerable<Clanarina> Search(string? pojam)
        {
            return _clanarinaRepository.Search(pojam);
        }

        public Clanarina? GetById(int clanarinaId)
        {
            return _clanarinaRepository.GetById(clanarinaId);
        }

        public bool Create(Clanarina clanarina, out string? errorMessage)
        {
            errorMessage = null;

            if (!ValidateClanarina(clanarina, out errorMessage))
            {
                return false;
            }

            _clanarinaRepository.Insert(clanarina);
            return true;
        }

        public bool Update(Clanarina clanarina, out string? errorMessage)
        {
            errorMessage = null;

            if (!ValidateClanarina(clanarina, out errorMessage))
            {
                return false;
            }

            _clanarinaRepository.Update(clanarina);
            return true;
        }

        public bool Delete(int clanarinaId, out string? errorMessage)
        {
            errorMessage = null;

            var existing = _clanarinaRepository.GetById(clanarinaId);
            if (existing is null)
            {
                errorMessage = "Članarina nije pronađena.";
                return false;
            }

            var povezaneUplate = _uplataRepository.GetByClanarinaId(clanarinaId);
            if (povezaneUplate.Any())
            {
                errorMessage = "Članarinu nije moguće obrisati jer postoje povezane uplate. Ako članarina više nije aktivna, promijenite njezin status u istekla ili otkazana.";
                return false;
            }

            _clanarinaRepository.Delete(clanarinaId);
            return true;
        }

        private bool ValidateClanarina(Clanarina clanarina, out string? errorMessage)
        {
            errorMessage = null;

            var clan = _clanRepository.GetById(clanarina.ClanId);
            if (clan is null)
            {
                errorMessage = "Odabrani član ne postoji.";
                return false;
            }

            var paket = _paketRepository.GetById(clanarina.PaketId);
            if (paket is null)
            {
                errorMessage = "Odabrani paket ne postoji.";
                return false;
            }

            if (clanarina.DatumZavrsetka <= clanarina.DatumPocetka)
            {
                errorMessage = "Datum završetka mora biti nakon datuma početka.";
                return false;
            }

            if (!Enum.IsDefined(typeof(StatusClanarine), clanarina.Status))
            {
                errorMessage = "Status članarine nije valjan.";
                return false;
            }

            return true;
        }
    }
}