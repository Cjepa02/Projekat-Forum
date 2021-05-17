using System.Data.Entity;

namespace Forum.Models.Dal
{
    public class ForumContext : DbContext
    {
        public ForumContext() : base("Forum")
        {
        }

        public DbSet<Korisnik> korisniks { get; set; }
        public DbSet<Podforum> podforums { get; set; }
        public DbSet<Tema> temas { get; set; }
        public DbSet<Komentar> komentars { get; set; }
        public DbSet<Reakcija> reakcijas { get; set; }
    }
}