using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.Configurations
{
    public class MovieActorConfig : IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            builder.HasKey(p => new { p.MovieId, p.ActorId });

            builder.HasOne(ma => ma.Actor).WithMany(ma => ma.MovieActors).HasForeignKey(ma => ma.ActorId);

            builder.HasOne(ma => ma.Movie).WithMany(ma => ma.MoviesActors).HasForeignKey(ma => ma.MovieId);
        }
    }
}