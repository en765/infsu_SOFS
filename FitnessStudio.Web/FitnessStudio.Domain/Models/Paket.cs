namespace FitnessStudio.Domain.Models
{
    public class Paket
    {
        public int PaketId { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public int BrojTreninga { get; set; }
        public decimal Cijena { get; set; }
    }
}