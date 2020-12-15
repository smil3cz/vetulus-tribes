using GreenFoxAcademy.SpaceSettlers.Logging;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFoxAcademy.SpaceSettlers.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Kingdom> Kingdoms { get; set; }
        public DbSet<LogData> Logs { get; set; }
        public DbSet<Token> Whitelist { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Ship> Ships { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            //AppSettings.GetDataFromSettings();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Kingdom)
                .WithMany(k => k.Resources)
                .HasForeignKey(r => r.KingdomId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Kingdom)
                .WithOne(k => k.User)
                .HasForeignKey<Kingdom>(k => k.UserId);

            modelBuilder.Entity<Building>()
                .HasOne(b => b.Kingdom)
                .WithMany(k => k.Buildings)
                .HasForeignKey(b => b.KingdomId);

            modelBuilder.Entity<Ship>()
                .HasOne(s => s.Kingdom)
                .WithMany(k => k.Ships)
                .HasForeignKey(s => s.KingdomId);

            modelBuilder.Entity<Kingdom>().OwnsOne(p => p.Location);
        }
    }
}
