using Microsoft.EntityFrameworkCore;

namespace GalaxyAudioPlayer.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        
        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseLazyLoadingProxies();
    }
}