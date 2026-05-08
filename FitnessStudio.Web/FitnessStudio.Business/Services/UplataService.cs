using FitnessStudio.Business.Interfaces;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Business.Services
{
    public class UplataService : IUplataService
    {
        private readonly IUplataRepository _uplataRepository;
        private readonly IClanarinaRepository _clanarinaRepository;
        private readonly IPaketRepository _paketRepository;

        public UplataService(
            IUplataRepository uplataRepository,
            IClanarinaRepository clanarinaRepository,
            IPaketRepository paketRepository)
        {
            _uplataRepository = uplataRepository;
            _clanarinaRepository = clanarinaRepository;
            _paketRepository = paketRepository;
        }

        public IEnumerable<Uplata> GetByClanarinaId(int clanarinaId)
        {
            return _uplataRepository.GetByClanarinaId(clanarinaId);
        }

        public Uplata? GetById(int uplataId)
        {
            return _uplataRepository.GetById(uplataId);
        }

        public bool Create(Uplata uplata, out string? errorMessage)
        {
            errorMessage = null;

            if (!ValidateUplata(uplata, false, out errorMessage))
            {
                return false;
            }

            _uplataRepository.Insert(uplata);
            return true;
        }

        public bool Update(Uplata uplata, out string? errorMessage)
        {
            errorMessage = null;

            if (!ValidateUplata(uplata, true, out errorMessage))
            {
                return false;
            }

            _uplataRepository.Update(uplata);
            return true;
        }

        public bool Delete(int uplataId, out string? errorMessage)
        {
            errorMessage = null;

            var existing = _uplataRepository.GetById(uplataId);
            if (existing is null)
            {
                errorMessage = "Uplata nije pronađena.";
                return false;
            }

            _uplataRepository.Delete(uplataId);
            return true;
        }

        private bool ValidateUplata(Uplata uplata, bool isUpdate, out string? errorMessage)
        {
            errorMessage = null;

            var clanarina = _clanarinaRepository.GetById(uplata.ClanarinaId);
            if (clanarina is null)
            {
                errorMessage = "Odabrana članarina ne postoji.";
                return false;
            }

            if (uplata.Iznos <= 0)
            {
                errorMessage = "Iznos uplate mora biti veći od 0.";
                return false;
            }

            var paket = _paketRepository.GetById(clanarina.PaketId);
            if (paket is null)
            {
                errorMessage = "Povezani paket nije pronađen.";
                return false;
            }

            var trenutnaSuma = _uplataRepository.GetTotalForClanarina(uplata.ClanarinaId);

            if (isUpdate)
            {
                var staraUplata = _uplataRepository.GetById(uplata.UplataId);
                if (staraUplata is not null)
                {
                    trenutnaSuma -= staraUplata.Iznos;
                }
            }

            if (trenutnaSuma + uplata.Iznos > paket.Cijena)
            {
                errorMessage = "Ukupan iznos uplata ne smije biti veći od cijene paketa.";
                return false;
            }

            return true;
        }
    }
}