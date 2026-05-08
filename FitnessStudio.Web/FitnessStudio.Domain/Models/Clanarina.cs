namespace FitnessStudio.Domain.Models
{
    public class Clanarina
    {
        public int ClanarinaId { get; set; }
        public int ClanId { get; set; }
        public int PaketId { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public StatusClanarine Status { get; set; }
    }
}