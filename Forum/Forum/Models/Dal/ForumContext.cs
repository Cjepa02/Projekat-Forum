using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Forum.Models.Dal
{
    public class ForumContext : DbContext
    {
        public ForumContext() : base("Forum")
        {

        }
        public DbSet<Korisnik> korisniks { get; set; }
    }
}