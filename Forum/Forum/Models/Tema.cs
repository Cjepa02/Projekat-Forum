using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class Tema
    {
        public int Podforum { get; set; }
        public string Tekst { get; set; }
        public string Naslov { get; set; }
        public int Korisnik { get; set; }
        public int IdTema { get; set; }
        public DateTime DatumVreme { get; set; }
        public bool Izmenjen { get; set; }
    }
}