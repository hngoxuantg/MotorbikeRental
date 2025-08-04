using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.User;
using System.Reflection.Emit;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationRoles : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasKey(e => e.Id);
            builder.HasMany(e => e.UserCredentials)
                 .WithOne(e => e.Roles);
        }
    }
}
