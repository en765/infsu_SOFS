using FitnessStudio.Domain.Models;

namespace FitnessStudio.Business.Interfaces
{
    public interface IPaketService
    {
        IEnumerable<Paket> GetAll();
        IEnumerable<Paket> SearchByName(string? naziv);
        Paket? GetById(int paketId);
        bool Create(Paket paket, out string? errorMessage);
        bool Update(Paket paket, out string? errorMessage);
        bool Delete(int paketId, out string? errorMessage);
    }
}