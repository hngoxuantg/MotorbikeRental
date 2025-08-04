using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IStatisticsRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq.Expressions;

namespace MotorbikeRental.Infrastructure.Data.Repositories.StatisticsRepositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly MotorbikeRentalDbContext dbContext;
        public StatisticsRepository(MotorbikeRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> GetTotalCount<T>(Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = dbContext.Set<T>().AsNoTracking();
            if (expression != null)
                query = query.Where(expression);
            return await query.CountAsync(cancellationToken);
        }
        public async Task<decimal> GetTotalPrice<T>(Expression<Func<T, decimal>> expression, Expression<Func<T, bool>>? where = null, CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = dbContext.Set<T>().AsNoTracking();
            if (where != null)
                query = query.Where(where);
            return await query.SumAsync(expression, cancellationToken);
        }
        public async Task<decimal> GetRevenueByMonth(int month, int year, CancellationToken cancellationToken = default)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1);
            return await dbContext.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate < endDate)
                .SumAsync(p => p.Amount, cancellationToken);
        }
        public async Task<decimal> GetRevenueByDay(DateTime date, CancellationToken cancellationToken = default)
        {
            return await dbContext.Payments
                .Where(p => p.PaymentDate.Date == date.Date)
                .SumAsync(p => p.Amount, cancellationToken);
        }
        public async Task<decimal> GetRevenueByYear(int year, CancellationToken cancellationToken = default)
        {
            return await dbContext.Payments
                .Where(p => p.PaymentDate.Year == year)
                .SumAsync(p => p.Amount, cancellationToken);
        }
        public async Task<DateTime> GetSystemStartDate(CancellationToken cancellation = default)
        {
            return await dbContext.RentalContracts
                .OrderBy(c => c.RentalDate)
                .Select(c => c.RentalDate)
                .FirstOrDefaultAsync(cancellation);
        }
        public async Task<IEnumerable<Employee>> GetSalaries(CancellationToken cancellationToken = default)
        {
            return await dbContext.Employees.AsNoTracking()
                .Select(e => new Employee
                {
                    StartDate = e.StartDate,
                    Salary = e.Salary,
                }).ToListAsync(cancellationToken);
        }
        public async Task<(IEnumerable<Payment>, IEnumerable<MaintenanceRecord>)> GetPaymentsAndMaintenanceRecords(DateTime toDate, DateTime endDate, CancellationToken cancellation = default)
        {
            IQueryable<Payment> paymentQuery = dbContext.Payments.AsNoTracking();
            IQueryable<MaintenanceRecord> maintenanceQuery = dbContext.MaintenanceRecords.AsNoTracking();

            paymentQuery = paymentQuery.Where(p => p.PaymentDate.Date >= toDate.Date && p.PaymentDate.Date <= endDate.Date);
            maintenanceQuery = maintenanceQuery.Where(m => m.CreatedAt >= toDate.Date && m.CreatedAt <= endDate.Date);

            maintenanceQuery = maintenanceQuery.Select(m => new MaintenanceRecord
            {
                MaintenanceDate = m.MaintenanceDate,
                Cost = m.Cost,
            });

            paymentQuery = paymentQuery.Select(p => new Payment
            {
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
            });
            return (await paymentQuery.ToListAsync(cancellation), await maintenanceQuery.ToListAsync(cancellation));
        }
        public async Task<(IEnumerable<Employee>, IEnumerable<Motorbike>)> GetEmployeesAndMotorbikesHighlight(int month, int year, CancellationToken cancellation = default)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var employeesQuery = dbContext.Employees
                .AsNoTracking()
                .OrderByDescending(e => e.RentalContracts.Count(rc => rc.RentalDate >= startDate && rc.RentalDate < endDate))
                .Include(e => e.RentalContracts.Where(er => er.RentalDate >= startDate && er.RentalDate < endDate))
                .Take(5);

            var motorbikesQuery = dbContext.Motorbikes
                .AsNoTracking()
                .OrderByDescending(m => m.RentalContracts.Count(rc => rc.RentalDate >= startDate && rc.RentalDate < endDate))
                .Include(m => m.RentalContracts.Where(mr => mr.RentalDate >= startDate && mr.RentalDate < endDate))
                .Take(3);

            var employees = await employeesQuery.ToListAsync(cancellation);
            var motorbikes = await motorbikesQuery.ToListAsync(cancellation);

            return (employees, motorbikes);
        }
    }
}
