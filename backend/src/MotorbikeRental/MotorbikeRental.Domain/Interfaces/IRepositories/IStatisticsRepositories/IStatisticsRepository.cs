using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;
using System.Linq.Expressions;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IStatisticsRepositories
{
    public interface IStatisticsRepository
    {
        Task<int> GetTotalCount<T>(Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default) where T : class;
        Task<decimal> GetRevenueByMonth(int month, int year, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalPrice<T>(Expression<Func<T, decimal>> expression, Expression<Func<T, bool>>? where = null, CancellationToken cancellationToken = default) where T : class;
        Task<DateTime> GetSystemStartDate(CancellationToken cancellation = default);
        Task<IEnumerable<Employee>> GetSalaries(CancellationToken cancellationToken = default);
        Task<(IEnumerable<Payment>, IEnumerable<MaintenanceRecord>)> GetPaymentsAndMaintenanceRecords(DateTime toDate, DateTime endDate, CancellationToken cancellation = default);
        Task<(IEnumerable<Employee>, IEnumerable<Motorbike>)> GetEmployeesAndMotorbikesHighlight(int month, int year, CancellationToken cancellation = default);
    }
}
