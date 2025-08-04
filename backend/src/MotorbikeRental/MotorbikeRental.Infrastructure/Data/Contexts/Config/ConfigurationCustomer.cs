using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotorbikeRental.Domain.Entities.Customers;

namespace MotorbikeRental.Infrastructure.Data.Contexts.Config
{
    public class ConfigurationCustomer : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(e => e.CustomerId);
            builder.Property(e => e.CustomerId).ValueGeneratedOnAdd();
            builder.Property(e => e.CreateAt)
                 .HasDefaultValueSql("GETDATE()");
            builder.HasMany(e => e.RentalContracts)
                .WithOne(e => e.Customer);
            builder.Property(e => e.Gender)
                .HasConversion<string>();
        }
    }
}
