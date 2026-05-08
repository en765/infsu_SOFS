using FitnessStudio.Domain.Models;

namespace FitnessStudio.Business.Interfaces
{
    public interface IClanarinaService
    {
        IEnumerable<Clanarina> GetAll();
        IEnumerable<Clanarina> Search(string? pojam);
        Clanarina? GetById(int clanarinaId);
        bool Create(Clanarina clanarina, out string? errorMessage);
        bool Update(Clanarina clanarina, out string? errorMessage);
        bool Delete(int clanarinaId, out string? errorMessage);
    }
}