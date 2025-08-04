using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Pricing;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationPriceList : IEntityTypeConfiguration<PriceList>
    {
        public void Configure(EntityTypeBuilder<PriceList> builder)
        {
            builder.ToTable("PriceList");
            builder.HasKey(e => e.PriceListId);
            builder.Property(e => e.PriceListId).ValueGeneratedOnAdd();
            builder.HasOne(e => e.Motorbike)
                .WithOne(e => e.PriceList)
                .HasForeignKey<PriceList>(e => e.MotorbikeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
