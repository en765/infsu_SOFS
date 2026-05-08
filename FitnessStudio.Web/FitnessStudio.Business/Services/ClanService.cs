using FitnessStudio.Business.Interfaces;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Business.Services
{
    public class ClanService : IClanService
    {
        private readonly IClanRepository _clanRepository;

        public ClanService(IClanRepository clanRepository)
        {
            _clanRepository = clanRepository;
        }

        public IEnumerable<Clan> GetAll()
        {
            return _clanRepository.GetAll();
        }

        public Clan? GetById(int clanId)
        {
            return _clanRepository.GetById(clanId);
        }
    }
}