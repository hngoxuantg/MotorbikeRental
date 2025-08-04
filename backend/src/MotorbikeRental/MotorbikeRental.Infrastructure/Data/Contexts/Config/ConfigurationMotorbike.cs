using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationMotorbike : IEntityTypeConfiguration<Motorbike>
    {
        public void Configure(EntityTypeBuilder<Motorbike> builder)
        {
            builder.ToTable("Motorbike");
            builder.HasKey(e => e.MotorbikeId);
            builder.Property(e => e.MotorbikeId).ValueGeneratedOnAdd();
            builder.HasOne(e => e.Category)
                .WithMany(e => e.Motorbikes)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.MaintenanceRecords)
                .WithOne(e => e.Motorbike);
            builder.Property(e => e.Status)
                .HasConversion<string>();
            builder.Property(e => e.MotorbikeConditionStatus)
                .HasConversion<string>();
            builder.HasOne(e => e.MotorbikeMaintenanceInfo)
                .WithOne(e => e.Motorbike);
        }
    }
}
