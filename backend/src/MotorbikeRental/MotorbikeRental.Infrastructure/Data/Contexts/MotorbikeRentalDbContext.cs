using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Infrastructure.Data.Contexts
{
    public class MotorbikeRentalDbContext : IdentityDbContext<UserCredentials, Roles, int>
    {
        public MotorbikeRentalDbContext(DbContextOptions<MotorbikeRentalDbContext> options) : base(options)
        {
        }
        #region DbSet Section
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Motorbike> Motorbikes { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Discount_Category> Discount_Categories { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RentalContract> RentalContracts { get; set; }
        public DbSet<IncidentImage> IncidentImages { get; set; }
        public DbSet<MotorbikeMaintenanceInfo> MotorbikeMaintenanceInfos { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MotorbikeRentalDbContext).Assembly);
        }
    }
}