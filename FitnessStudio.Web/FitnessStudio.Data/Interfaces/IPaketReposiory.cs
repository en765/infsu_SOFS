using FitnessStudio.Domain.Models;

namespace FitnessStudio.Data.Interfaces
{
    public interface IPaketRepository
    {
        IEnumerable<Paket> GetAll();
        Paket? GetById(int paketId);
        IEnumerable<Paket> SearchByName(string? naziv);
        void Insert(Paket paket);
        void Update(Paket paket);
        void Delete(int paketId);
        bool ExistsByName(string naziv);
        bool ExistsByName(string naziv, int paketId);
    }
}