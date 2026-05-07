using FitnessStudio.Business.Interfaces;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Business.Services
{
    public class PaketService : IPaketService
    {
        private readonly IPaketRepository _paketRepository;

        public PaketService(IPaketRepository paketRepository)
        {
            _paketRepository = paketRepository;
        }

        public IEnumerable<Paket> GetAll()
        {
            return _paketRepository.GetAll();
        }

        public IEnumerable<Paket> SearchByName(string? naziv)
        {
            return _paketRepository.SearchByName(naziv);
        }

        public Paket? GetById(int paketId)
        {
            return _paketRepository.GetById(paketId);
        }

        public bool Create(Paket paket, out string? errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(paket.Naziv))
            {
                errorMessage = "Naziv paketa je obavezan.";
                return false;
            }

            if (paket.BrojTreninga <= 0)
            {
                errorMessage = "Broj treninga mora biti veći od 0.";
                return false;
            }

            if (paket.Cijena <= 0)
            {
                errorMessage = "Cijena mora biti veća od 0.";
                return false;
            }

            if (_paketRepository.ExistsByName(paket.Naziv))
            {
                errorMessage = "Paket s istim nazivom već postoji.";
                return false;
            }

            _paketRepository.Insert(paket);
            return true;
        }

        public bool Update(Paket paket, out string? errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(paket.Naziv))
            {
                errorMessage = "Naziv paketa je obavezan.";
                return false;
            }

            if (paket.BrojTreninga <= 0)
            {
                errorMessage = "Broj treninga mora biti veći od 0.";
                return false;
            }

            if (paket.Cijena <= 0)
            {
                errorMessage = "Cijena mora biti veća od 0.";
                return false;
            }

            if (_paketRepository.ExistsByName(paket.Naziv, paket.PaketId))
            {
                errorMessage = "Paket s istim nazivom već postoji.";
                return false;
            }

            _paketRepository.Update(paket);
            return true;
        }

        public bool Delete(int paketId, out string? errorMessage)
        {
            errorMessage = null;

            var paket = _paketRepository.GetById(paketId);
            if (paket is null)
            {
                errorMessage = "Paket nije pronađen.";
                return false;
            }

            _paketRepository.Delete(paketId);
            return true;
        }
    }
}