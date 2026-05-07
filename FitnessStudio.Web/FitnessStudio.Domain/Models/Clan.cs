namespace FitnessStudio.Domain.Models
{
    public class Clan
    {
        public int ClanId { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;

        public string PunoIme => $"{Ime} {Prezime}";
    }
}