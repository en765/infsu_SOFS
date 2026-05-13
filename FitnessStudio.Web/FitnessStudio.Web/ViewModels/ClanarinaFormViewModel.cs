using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessStudio.Domain.Models;

namespace FitnessStudio.Web.ViewModels
{
    public class ClanarinaFormViewModel
    {
        public int ClanarinaId { get; set; }

        [Required(ErrorMessage = "Potrebno je odabrati člana.")]
        public int? ClanId { get; set; }

        [Required(ErrorMessage = "Potrebno je odabrati paket.")]
        public int? PaketId { get; set; }

        [Required(ErrorMessage = "Datum početka je obavezan.")]
        [DataType(DataType.Date)]
        public DateTime DatumPocetka { get; set; }

        [Required(ErrorMessage = "Datum završetka je obavezan.")]
        [DataType(DataType.Date)]
        public DateTime DatumZavrsetka { get; set; }

        [Required(ErrorMessage = "Status je obavezan.")]
        public StatusClanarine Status { get; set; }

        public List<SelectListItem> Clanovi { get; set; } = new();
        public List<SelectListItem> Paketi { get; set; } = new();
    }
}