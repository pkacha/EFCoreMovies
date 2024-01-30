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
            Module6Seeding.Seed(modelBuilder);

            modelBuilder.Entity<CinemaWithoutLocation>().ToSqlQuery("SELECT Id, Name FROM Cinemas").ToView(null);

            modelBuilder.Entity<MovieWithCounts>().ToView("MoviesWithCounts");

            modelBuilder.Entity<Merchandising>().ToTable("Merchandising");

            modelBuilder.Entity<RentableMovie>().ToTable("RentableMovies");

            var movie1 = new RentableMovie()
            {
                Id = 1,
                Name = "Spider-Man",
                MovieId = 1,
                Price = 5.99m
            };

            var merch1 = new Merchandising()
            {
                Id = 2,
                Available = true,
                IsClothing = true,
                Name = "One Piece T-Shirt",
                Weight = 1,
                Volume = 1,
                Price = 11
            };

            modelBuilder.Entity<Merchandising>().HasData(merch1);
            modelBuilder.Entity<RentableMovie>().HasData(movie1);
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<CinemaDetail> CinemaDetails { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<CinemaOffer> CinemaOffers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}