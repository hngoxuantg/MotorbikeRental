using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Pricing;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationDiscount : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discount");
            builder.Property(e => e.DiscountId).ValueGeneratedOnAdd();
            builder.HasKey(e => e.DiscountId);
            builder.HasMany(e => e.Categories)
                .WithOne(e => e.Discount);
            builder.Property(e => e.StartDate)
                .HasDefaultValueSql("GETDATE()");
            builder.HasMany(e => e.Contracts)
                .WithOne(e => e.Discount);
        }
    }
}
