using FitnessStudio.Domain.Models;

namespace FitnessStudio.Data.Interfaces
{
    public interface IClanRepository
    {
        IEnumerable<Clan> GetAll();
        Clan? GetById(int clanId);
    }
}