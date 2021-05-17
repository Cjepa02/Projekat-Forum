namespace Forum.Models
{
    public class Reakcija
    {
        public int Id { get; set; }
        public int KomentarId { get; set; }
        public enum Tip { Srce, Smajli, Plač, Stidljivko, Mrgud, Like, Dislike }
        public Tip tip { get; set; }
        public int KorisnikId { get; set; }
        public string KorisnikKorisnickoIme { get; set; }
    }
}