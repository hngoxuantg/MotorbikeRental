using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Contract;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationPayment : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");
            builder.HasKey(e => e.PaymentId);
            builder.Property(e => e.PaymentId).ValueGeneratedOnAdd();
            builder.HasOne(e => e.RentalContract)
                .WithOne(e => e.Payments)
                .HasForeignKey<Payment>(e => e.ContractId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(e => e.PaymentStatus)
                .HasConversion<string>();
            builder.HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
