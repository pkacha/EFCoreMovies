using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.Configurations
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Amount).HasPrecision(18, 2);

            builder.HasDiscriminator(p => p.PaymentType)
                .HasValue<PayPalPayment>(PaymentType.PayPal)
                .HasValue<CardPayment>(PaymentType.Card);
        }
    }
}