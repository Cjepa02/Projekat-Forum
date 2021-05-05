namespace Forum.Models
{
    public class Podforum
    {
        public string Naslov { get; set; }
        public int Id { get; set; }
        public int KorisnikId { get; set; }
        public string KorisnikKorisnickoIme { get; set; }
    }
}