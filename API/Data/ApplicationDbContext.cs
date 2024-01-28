using System.Reflection;
using API.Entities;
using API.Entities.Keyless;
using API.Entities.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("Date");
            configurationBuilder.Properties<string>().HaveMaxLength(150);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            Module3Seeding.Seed(modelBuilder);

            //modelBuilder.Entity<Log>().Property(l => l.Id).ValueGeneratedNever();
            modelBuilder.Ignore<Address>();

            modelBuilder.Entity<CinemaWithoutLocation>().ToSqlQuery("SELECT Id, Name FROM Cinemas").ToView(null);

            modelBuilder.Entity<MovieWithCounts>().ToView("MoviesWithCounts");

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.Name.Contains("URL", StringComparison.CurrentCultureIgnoreCase))
                        property.SetIsUnicode(false);
                }
            }
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<CinemaOffer> CinemaOffers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
    }
}