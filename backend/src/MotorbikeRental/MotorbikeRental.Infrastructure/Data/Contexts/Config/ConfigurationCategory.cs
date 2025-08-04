using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationCategory : IEntityTypeConfiguration<Category> 
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(e => e.CategoryId);
            builder.Property(e => e.CategoryId).ValueGeneratedOnAdd();
            builder.HasMany(e => e.Motorbikes)
                .WithOne(e => e.Category);
            builder.HasMany(e => e.Discounts)
                .WithOne(e => e.Category);
        }
    }
}
