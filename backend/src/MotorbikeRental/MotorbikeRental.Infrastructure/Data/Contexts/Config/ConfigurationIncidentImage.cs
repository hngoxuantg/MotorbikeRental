using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Incidents;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationIncidentImage: IEntityTypeConfiguration<IncidentImage>
    {
        public void Configure(EntityTypeBuilder<IncidentImage> builder)
        {
            builder.ToTable("IncidentImage");
            builder.HasKey(e => e.ImageId);
            builder.Property(e => e.ImageId).ValueGeneratedOnAdd();
            builder.HasOne(e => e.Incident)
                .WithMany(e => e.Images)
                .HasForeignKey(e => e.IncidentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
