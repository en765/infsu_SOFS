using FitnessStudio.Domain.Models;

namespace FitnessStudio.Data.Interfaces
{
    public interface IClanarinaRepository
    {
        IEnumerable<Clanarina> GetAll();
        Clanarina? GetById(int clanarinaId);
        IEnumerable<Clanarina> Search(string? pojam);
        void Insert(Clanarina clanarina);
        void Update(Clanarina clanarina);
        void Delete(int clanarinaId);
    }
}