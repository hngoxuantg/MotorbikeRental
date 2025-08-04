using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Enums.UserEnum;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Employee>, int totalCount)> GetFilterData(int? employeeId, string? search, int pageNumber, int pageSize, int? RoleId, EmployeeStatus? status, CancellationToken cancellation = default);
        Task<Employee?> GetEmployeeBasicInfoById(int employeeId, CancellationToken cancellationToken = default);
    }
}