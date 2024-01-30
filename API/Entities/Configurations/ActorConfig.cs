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

            /* builder.OwnsOne(a => a.HomeAddress, ha =>
            {
                ha.Property(p => p.Street).HasColumnName("Street");
                ha.Property(p => p.Province).HasColumnName("Province");
                ha.Property(p => p.Country).HasColumnName("Country");
            });

            builder.OwnsOne(a => a.BillingAddress, ba =>
            {
                ba.Property(p => p.Street).HasColumnName("Street");
                ba.Property(p => p.Province).HasColumnName("Province");
                ba.Property(p => p.Country).HasColumnName("Country");
            }); */
        }
    }
}