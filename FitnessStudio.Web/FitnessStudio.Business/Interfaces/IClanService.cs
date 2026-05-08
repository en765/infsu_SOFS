using FitnessStudio.Domain.Models;

namespace FitnessStudio.Business.Interfaces
{
    public interface IClanService
    {
        IEnumerable<Clan> GetAll();
        Clan? GetById(int clanId);
    }
}