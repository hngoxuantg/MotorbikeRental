using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Pricing;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationDiscount_Category : IEntityTypeConfiguration<Discount_Category>
    {
        public void Configure(EntityTypeBuilder<Discount_Category> builder)
        {
            builder.ToTable("Discount_Category");
            builder.HasKey(e => new { e.DiscountId, e.CategoryId });
            builder.HasOne(e => e.Category)
                .WithMany(e => e.Discounts)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Discount)
                .WithMany(e => e.Categories)
                .HasForeignKey(e => e.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
