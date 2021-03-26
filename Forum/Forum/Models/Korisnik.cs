using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class Korisnik
    {
        public string ime { get; set; }
        public string prezime { get; set; }
        public string korisnickoime { get; set; }
        public string lozinka { get; set; }s
        public string tip_korisnika { get; set; } //Će metnemo enum ako bude trebalo
        public string email { get; set; }
        public int idkorisnika { get; set; }
    }
}