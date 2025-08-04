using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Enums.UserEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.UserRepositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Employee> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Employees.AsNoTracking()
                .Where(e => e.EmployeeId == id)
                .Include(e => e.UserCredentials)
                .ThenInclude(u => u.Roles)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Employee with id{id} not found");
        }
        public async Task<(IEnumerable<Employee>, int totalCount)> GetFilterData(int? employeeId, string? search, int pageNumber, int pageSize, int? RoleId, EmployeeStatus? status, CancellationToken cancellation = default)
        {
            Expression<Func<Employee, bool>> filter = e =>
                (employeeId == null || e.EmployeeId != employeeId) &&
                (RoleId == null || e.UserCredentials.RoleId == RoleId) &&
                (status == null || e.Status == status) &&
                (string.IsNullOrWhiteSpace(search) ||
                 e.FullName.ToLower().Contains(search.ToLower()) ||
                 e.UserCredentials.PhoneNumber.Contains(search) ||
                 e.UserCredentials.Email.Contains(search));

            return await GetPaged(filter,
                query => query.OrderByDescending(e => e.EmployeeId),
                pageNumber,
                pageSize,
                cancellation,
                e => e.UserCredentials,
                e => e.UserCredentials.Roles);
        }
        public async Task<Employee?> GetEmployeeBasicInfoById(int employeeId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Employees.AsNoTracking()
                .Where(e => e.EmployeeId == employeeId)
                .Select(e => new Employee
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}