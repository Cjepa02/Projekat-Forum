using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models
{
    public class Korisnik
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string Ime { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string Prezime { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string KorisnickoIme { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string Lozinka { get; set; }

        public string TrenutnaLozinka { get; set; }
        public string Tip_korisnika { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Email { get; set; }
    }
}