using FitnessStudio.Domain.Models;

namespace FitnessStudio.Business.Interfaces
{
    public interface IUplataService
    {
        IEnumerable<Uplata> GetByClanarinaId(int clanarinaId);
        Uplata? GetById(int uplataId);
        bool Create(Uplata uplata, out string? errorMessage);
        bool Update(Uplata uplata, out string? errorMessage);
        bool Delete(int uplataId, out string? errorMessage);
    }
}