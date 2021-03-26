using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class Reakcija
    {
        public enum Tip { srce, smajli, place, stidljivko, mrgud, like, dislike }
        public int IdReakcija { get; set; }
        public Tip tip { get; set; }
        public int IdKomentar { get; set; }
        public int IdKorisnik { get; set; }
    }
}