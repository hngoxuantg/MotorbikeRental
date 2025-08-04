using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationMaintenanceRecord : IEntityTypeConfiguration<MaintenanceRecord>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
        {
            builder.ToTable("MaintenanceRecord");
            builder.HasKey(e => e.MaintenanceRecordId);
            builder.Property(e => e.MaintenanceRecordId).ValueGeneratedOnAdd();
            builder.HasOne(e => e.Motorbike)
                .WithMany(e => e.MaintenanceRecords)
                .HasForeignKey(e => e.MotorbikeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Employee)
                .WithMany(e => e.MaintenanceRecords)
                .HasForeignKey(e => e.EmployeeId);
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
