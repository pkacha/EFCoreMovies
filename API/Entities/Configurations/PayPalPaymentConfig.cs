using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.Configurations
{
    public class PayPalPaymentConfig : IEntityTypeConfiguration<PayPalPayment>
    {
        public void Configure(EntityTypeBuilder<PayPalPayment> builder)
        {
            builder.Property(p => p.EmailAddress).IsRequired();

            var payment1 = new PayPalPayment()
            {
                Id = 1,
                PaymentDate = new DateTime(2024, 1, 30),
                PaymentType = PaymentType.PayPal,
                Amount = 123,
                EmailAddress = "abc@hotmail.com"
            };

            var payment2 = new PayPalPayment()
            {
                Id = 2,
                PaymentDate = new DateTime(2024, 1, 31),
                PaymentType = PaymentType.PayPal,
                Amount = 456,
                EmailAddress = "xyz@hotmail.com"
            };

            builder.HasData(payment1, payment2);
        }
    }
}