using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Dto
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Korisnicko ime mora imati vrednost!")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Lozinka mora imati vrednost!")]
        public string Lozinka { get; set; }
    }
}