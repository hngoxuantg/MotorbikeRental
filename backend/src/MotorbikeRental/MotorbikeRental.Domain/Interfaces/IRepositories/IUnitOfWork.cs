using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IStatisticsRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Domain.Interfaces.IRepositories;

public interface IUnitOfWork
{
    IPaymentRepository PaymentRepository { get; }
    IRentalContractRepository RentalContractRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IIncidentRepository IncidentRepository { get; }
    IIncidentImageRepository IncidentImageRepository { get; }
    IDiscountRepository DiscountRepository { get; }
    IPriceListRepository PriceListRepository { get; }
    IStatisticsRepository StatisticsRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IRoleRepository RoleRepository { get; }
    IUserCredentialsRepository UserCredentialsRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IMotorbikeRepository MotorbikeRepository { get; }
    IMaintenanceRecordRepository MaintenanceRecordRepository { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    void Dispose();
}