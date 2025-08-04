using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationEmployee : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");
            builder.HasKey(e => e.EmployeeId);
            builder.Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();
            builder.Property(e => e.Status)
                .HasConversion<string>();
            builder.HasOne(e => e.UserCredentials)
                .WithOne(e => e.Employee);
            builder.HasMany(e => e.MaintenanceRecords)
                .WithOne(e => e.Employee);
        }
    }
}
