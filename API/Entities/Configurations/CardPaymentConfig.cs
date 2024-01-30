using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.Configurations
{
    public class CardPaymentConfig : IEntityTypeConfiguration<CardPayment>
    {
        public void Configure(EntityTypeBuilder<CardPayment> builder)
        {
            builder.Property(p => p.Last4Digits).HasColumnType("char(4)").IsRequired();

            var payment1 = new CardPayment()
            {
                Id = 3,
                PaymentDate = new DateTime(2023, 12, 14),
                PaymentType = PaymentType.Card,
                Amount = 15.99m,
                Last4Digits = "4567"
            };

            var payment2 = new CardPayment()
            {
                Id = 4,
                PaymentDate = new DateTime(2023, 12, 15),
                PaymentType = PaymentType.Card,
                Amount = 19,
                Last4Digits = "1111"
            };

            builder.HasData(payment1, payment2);
        }
    }
}