using Microsoft.EntityFrameworkCore.Storage;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IStatisticsRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MotorbikeRentalDbContext dbContext;
    private IDbContextTransaction? transaction;

    public UnitOfWork(
        IPaymentRepository paymentRepository,
        IRentalContractRepository rentalContractRepository,
        ICustomerRepository customerRepository,
        IIncidentRepository incidentRepository,
        IIncidentImageRepository incidentImageRepository,
        IDiscountRepository discountRepository,
        IPriceListRepository priceListRepository,
        IStatisticsRepository statisticsRepository,
        IEmployeeRepository employeeRepository,
        IRoleRepository roleRepository,
        IUserCredentialsRepository userCredentialsRepository,
        ICategoryRepository categoryRepository,
        IMotorbikeRepository motorbikeRepository,
        IMaintenanceRecordRepository maintenanceRecordRepository,
        MotorbikeRentalDbContext dbContext)
    {
        this.dbContext = dbContext;
        PaymentRepository = paymentRepository;
        RentalContractRepository = rentalContractRepository;
        CustomerRepository = customerRepository;
        IncidentRepository = incidentRepository;
        IncidentImageRepository = incidentImageRepository;
        DiscountRepository = discountRepository;
        PriceListRepository = priceListRepository;
        StatisticsRepository = statisticsRepository;
        EmployeeRepository = employeeRepository;
        RoleRepository = roleRepository;
        UserCredentialsRepository = userCredentialsRepository;
        CategoryRepository = categoryRepository;
        MotorbikeRepository = motorbikeRepository;
        MaintenanceRecordRepository = maintenanceRecordRepository;
    }
    public IPaymentRepository PaymentRepository { get; }
    public IRentalContractRepository RentalContractRepository { get; }
    public ICustomerRepository CustomerRepository { get; }
    public IIncidentRepository IncidentRepository { get; }
    public IIncidentImageRepository IncidentImageRepository { get; }
    public IDiscountRepository DiscountRepository { get; }
    public IPriceListRepository PriceListRepository { get; }
    public IStatisticsRepository StatisticsRepository { get; }
    public IEmployeeRepository EmployeeRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IUserCredentialsRepository UserCredentialsRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IMotorbikeRepository MotorbikeRepository { get; }
    public IMaintenanceRecordRepository MaintenanceRecordRepository { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction != null)
        {
            await transaction.CommitAsync(cancellationToken);
            transaction.Dispose();
            transaction = null;
        }
    }
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction != null)
        {
            await transaction.RollbackAsync(cancellationToken);
            await transaction.DisposeAsync();
            transaction = null;
        }
    }

    public void Dispose()
    {
        transaction?.Dispose();
        dbContext?.Dispose();
    }
}