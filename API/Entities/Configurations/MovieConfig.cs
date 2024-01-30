using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.Configurations
{
    public class MovieConfig : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(250).IsRequired();
            builder.Property(p => p.PosterURL).HasMaxLength(500).IsUnicode(false);

            builder.HasMany(m => m.Genres).WithMany(g => g.Movies);
           //     .UsingEntity(mg => mg.ToTable("GenresMovies").HasData(new { MoviesId = 1, GenresId = 7 }));
        }
    }
}