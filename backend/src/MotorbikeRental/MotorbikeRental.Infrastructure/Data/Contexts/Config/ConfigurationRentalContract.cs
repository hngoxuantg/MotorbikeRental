using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Contract;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationRentalContract : IEntityTypeConfiguration<RentalContract>
    {
        public void Configure(EntityTypeBuilder<RentalContract> builder)
        {
            builder.ToTable("RentalContract");
            builder.HasKey(e => e.ContractId);
            builder.Property(e => e.ContractId).ValueGeneratedOnAdd();
            builder.HasOne(e => e.Customer)
                .WithMany(e => e.RentalContracts)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Property(e => e.RentalDate)
                .HasDefaultValueSql("GETDATE()");
            builder.HasOne(e => e.Motorbike)
                .WithMany(e => e.RentalContracts)
                .HasForeignKey(e => e.MotorbikeId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Property(e => e.RentalContractStatus)
                .HasConversion<string>();
            builder.HasOne(e => e.Employee)
                .WithMany(e => e.RentalContracts)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.RentalTypeStatus)
                .HasConversion<string>();
            builder.HasOne(e => e.Discount)
                .WithMany(e => e.Contracts)
                .HasForeignKey(e => e.DiscountId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
