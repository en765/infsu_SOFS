using FitnessStudio.Domain.Models;

namespace FitnessStudio.Data.Interfaces
{
    public interface IUplataRepository
    {
        IEnumerable<Uplata> GetByClanarinaId(int clanarinaId);
        Uplata? GetById(int uplataId);
        void Insert(Uplata uplata);
        void Update(Uplata uplata);
        void Delete(int uplataId);
        decimal GetTotalForClanarina(int clanarinaId);
    }
}