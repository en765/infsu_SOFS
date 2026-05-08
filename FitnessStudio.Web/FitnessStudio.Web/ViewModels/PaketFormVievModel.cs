using System.ComponentModel.DataAnnotations;

namespace FitnessStudio.Web.ViewModels
{
    public class PaketFormViewModel
    {
        public int PaketId { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan.")]
        public string Naziv { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Broj treninga mora biti veći od 0.")]
        public int BrojTreninga { get; set; }

        [Required(ErrorMessage = "Cijena je obavezna.")]
        public string Cijena { get; set; } = string.Empty;
    }
}