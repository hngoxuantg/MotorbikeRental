using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Incidents;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationIncident : IEntityTypeConfiguration<Incident>
    {
        public void Configure(EntityTypeBuilder<Incident> builder)
        {
            builder.ToTable("Incident");
            builder.HasKey(e => e.IncidentId);
            builder.Property(e => e.IncidentId).ValueGeneratedOnAdd();
            builder.HasOne(e => e.RentalContract)
                .WithOne(e => e.Incident)
                .HasForeignKey<Incident>(e => e.ContractId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Property(e => e.Type)
                .HasConversion<string>();
            builder.HasOne(e => e.Motorbike)
                .WithMany(e => e.Incidents)
                .HasForeignKey(e => e.MotorbikeId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Property(e => e.Severity)
                .HasConversion<string>();
            builder.HasOne(e => e.ReportedByEmployee)
                .WithMany(e => e.Incidents)
                .HasForeignKey(e => e.ReportedByEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(e => e.Images)
                .WithOne(e => e.Incident)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
