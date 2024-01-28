using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.Configurations
{
    public class ActorConfig : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Biography).HasColumnType("nvarchar(max)");
            //builder.Property(p => p.Name).HasField("_name");
            //builder.Ignore(p => p.Age);
        }
    }
}