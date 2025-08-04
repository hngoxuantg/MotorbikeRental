using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationMotorbikeMaintenanceInfo : IEntityTypeConfiguration<MotorbikeMaintenanceInfo>
    {
        public void Configure(EntityTypeBuilder<MotorbikeMaintenanceInfo> builder)
        {
            builder.ToTable("MotorbikeMaintenanceInfo");
            builder.HasKey(e => e.MotorbikeId);
            builder.HasOne(e => e.Motorbike)
                .WithOne(e => e.MotorbikeMaintenanceInfo)
                .HasForeignKey<MotorbikeMaintenanceInfo>(e => e.MotorbikeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
