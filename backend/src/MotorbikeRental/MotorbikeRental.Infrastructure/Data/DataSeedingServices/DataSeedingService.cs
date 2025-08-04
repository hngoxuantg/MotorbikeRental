using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MotorbikeRental.Application.Interface.IDataSeedingServices;
using MotorbikeRental.Common.Options;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.UserEnum;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.DataSeedingServices
{
    public class DataSeedingService : IDataSeedingService
    {
        private readonly MotorbikeRentalDbContext dbContext;
        private readonly AdminAccount adminAccount;
        private readonly UserManager<UserCredentials> userManager;
        private readonly RoleManager<Roles> roleManager;
        public DataSeedingService(MotorbikeRentalDbContext dbContext, IOptions<AdminAccount> adminAccount, RoleManager<Roles> roleManager, UserManager<UserCredentials> userManager)
        {
            this.dbContext = dbContext;
            this.adminAccount = adminAccount.Value;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task SeedData()
        {
            if (!dbContext.Roles.Any())
            {
                List<Roles> roles = new List<Roles>()
                {
                    new Roles()
                    {
                        Description = "Quản lý toàn hệ thống, thống kê, duyệt giảm giá, phân quyền",
                        Name = "Manager",
                    },
                    new Roles()
                    {
                        Description = "Lễ tân lập và kiểm duyệt hợp đồng, xử lý thanh toán",
                        Name = "Receptionist",
                    },
                    new Roles()
                    {
                        Description = "Bảo trì, sửa chữa, ghi nhận chi phí bảo dưỡng",
                        Name = "Maintenance",
                    }
                };
                for(int i = 0; i < roles.Count; i++)
                    await roleManager.CreateAsync(roles[i]);
            }
            if (!dbContext.Employees.Any())
            {
                Employee employee = new Employee()
                {
                    FullName = adminAccount.Info.FullName,
                    DateOfBirth = adminAccount.Info.DateOfBirth.Value,
                    Address = adminAccount.Info.Address,
                    StartDate = DateTime.UtcNow,
                    Salary = 0,
                    Status = EmployeeStatus.Active,
                };
                await dbContext.Employees.AddAsync(employee);
                await dbContext.SaveChangesAsync();
            }
            if (!dbContext.UserCredentials.Any())
            {
                Roles roles = await roleManager.FindByNameAsync("Manager");
                UserCredentials userCredentials = new UserCredentials()
                {
                    RoleId = roles.Id,
                    EmployeeId = await dbContext.Employees.Where(e => e.FullName == adminAccount.Info.FullName).Select(e => e.EmployeeId).FirstOrDefaultAsync(),
                    UserName = adminAccount.Account.UserName,
                    Email = adminAccount.Account.Email,
                    PhoneNumber = adminAccount.Account.PhoneNumber,
                };
                IdentityResult result = await userManager.CreateAsync(userCredentials, adminAccount.Account.Password);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                await userManager.AddToRoleAsync(userCredentials, roles.Name);
            }
        }
        private async Task InsertContracts()
        {
            List<RentalContract> contracts = new List<RentalContract>();
            Random random = new Random(); 

            DateTime start = new DateTime(2020, 6, 1);
            DateTime maxRentalDate = DateTime.UtcNow.AddDays(-10); 

            int totalDaysRange = (maxRentalDate - start).Days;

            for (int i = 1; i <= 500; i++)
            {
                DateTime rentalDate = start.AddDays(random.Next(totalDaysRange + 1));

                DateTime returnDate = rentalDate.AddDays(random.Next(1, 11));

                contracts.Add(new RentalContract
                {
                    CustomerId = i,
                    MotorbikeId = random.Next(1, 4),
                    DepositAmount = 500_000,
                    EmployeeId = random.Next(2, 6),
                    RentalDate = rentalDate,
                    ExpectedReturnDate = returnDate,
                    TotalAmount = random.Next(100000, 1000000),
                    ActualReturnDate = returnDate,
                    RentalTypeStatus = Domain.Enums.ContractEnum.RentalTypeStatus.Hourly,
                    IdCardHeld = false,
                    IsPaid = true,
                });
            }

            dbContext.RentalContracts.AddRange(contracts);
            await dbContext.SaveChangesAsync();
        }
        private async Task InsertPayments()
        {
            List<Payment> payments = new List<Payment>();
            List<RentalContract> rentalContracts = await dbContext.RentalContracts.ToListAsync();
            Random random = new Random();

            for (int i = 0; i < rentalContracts.Count; i++)
            {
                payments.Add(new Payment
                {
                    ContractId = rentalContracts[i].ContractId,
                    Amount = rentalContracts[i].TotalAmount,
                    PaymentDate = rentalContracts[i].ExpectedReturnDate,
                    PaymentStatus = Domain.Enums.ContractEnum.PaymentStatus.Cash,
                    EmployeeId = random.Next(2, 6),
                });
            }
            dbContext.Payments.AddRange(payments);
            await dbContext.SaveChangesAsync();
        }
        private async Task InsertMaintenanceRecords()
        {
            List<MaintenanceRecord> maintenanceRecords = new List<MaintenanceRecord>();
            Random random = new Random();

            for (DateTime date = new DateTime(2021, 1, 1); date < DateTime.UtcNow.Date; date = date.AddMonths(6))
            {
                for(int i = 1; i <= 4; i++)
                {
                    maintenanceRecords.Add(new MaintenanceRecord
                    {
                        MotorbikeId = i,
                        MaintenanceDate = date,
                        Description = "Bảo dưỡng định kì ngày: " + date.ToString("dd/MM/yyyy"),
                        Cost = random.Next(100000, 500000),
                        EmployeeId = 3,
                        CreatedAt = date,
                    });
                }
            }
            dbContext.MaintenanceRecords.AddRange(maintenanceRecords);
            await dbContext.SaveChangesAsync();
        }
    }
}
