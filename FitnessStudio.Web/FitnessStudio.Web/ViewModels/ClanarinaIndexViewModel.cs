using FitnessStudio.Domain.Models;

namespace FitnessStudio.Web.ViewModels
{
    public class ClanarinaIndexViewModel
    {
        public IEnumerable<Clanarina> Clanarine { get; set; } = new List<Clanarina>();
        public IEnumerable<Uplata> Uplate { get; set; } = new List<Uplata>();

        public int? SelectedClanarinaId { get; set; }
        public string? Search { get; set; }

        public Dictionary<int, string> Clanovi { get; set; } = new();
        public Dictionary<int, string> Paketi { get; set; } = new();

        public string? OdabraniPaketNaziv { get; set; }
        public decimal? OdabraniPaketCijena { get; set; }
        public decimal UkupnoUplaceno { get; set; }
        public decimal PreostaloZaUplatu { get; set; }
    }
}