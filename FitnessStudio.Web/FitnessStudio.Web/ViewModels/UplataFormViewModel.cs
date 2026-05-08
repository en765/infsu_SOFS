using System.ComponentModel.DataAnnotations;

namespace FitnessStudio.Web.ViewModels
{
    public class UplataFormViewModel
    {
        public int UplataId { get; set; }

        [Required]
        public int ClanarinaId { get; set; }

        [Required(ErrorMessage = "Iznos je obavezan.")]
        public string Iznos { get; set; } = string.Empty;

        [Required(ErrorMessage = "Datum je obavezan.")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
    }
}