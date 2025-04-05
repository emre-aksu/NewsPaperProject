using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;

namespace DataAccessLayer.Context
{
    public class NewsPaperDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                 (@"Server=Monster;Database=GazeteWeb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Economy> Economies { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<Special> Specials { get; set; }
        public DbSet<Sport> Sports { get; set; }
    }
}

