using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class Komentar
    {
        public int IdTema { get; set; }
        public int IdKorisnik { get; set; }
        public string Tekst { get; set; }
        public DateTime Kreiranje { get; set; }
        public bool Izmenjen { get; set; }
        public int IdKomentar { get; set; }
    }
}