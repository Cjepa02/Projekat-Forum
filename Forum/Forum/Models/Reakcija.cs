using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class Reakcija
    {
        public int Id { get; set; }
        public enum Tip { Srce, Smajli, Plač, Stidljivko, Mrgud, Like, Dislike }
        public Tip tip { get; set; }
        public int KorisnikId { get; set; }
    }
}