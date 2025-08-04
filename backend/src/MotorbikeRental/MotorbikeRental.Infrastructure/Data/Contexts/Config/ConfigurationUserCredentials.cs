using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationUserCredentials : IEntityTypeConfiguration<UserCredentials>
    {
        public void Configure(EntityTypeBuilder<UserCredentials> builder)
        {
            builder.ToTable("UserCredentials");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne(e => e.Employee)
                .WithOne(e => e.UserCredentials)
                .HasForeignKey<UserCredentials>(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(e => e.Roles)
                .WithMany(e => e.UserCredentials)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
