using System;

namespace Forum.Models
{
    public class Komentar
    {
        public int Id { get; set; }
        public int TemaId { get; set; }
        public int KorisnikId { get; set; }
        public string KorisnikKorisnickoIme { get; set; }
        public string Tekst { get; set; }
        public DateTime Kreiranje { get; set; }
        public bool Izmenjen { get; set; }
    }
}