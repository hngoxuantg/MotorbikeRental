using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Interface.IServices.IUserServices
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateEmployee(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default);
        Task<EmployeeDto> GetEmployeeById(int id, CancellationToken cancellation = default);
        Task<EmployeeDto> UpdateEmployee(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellation = default);
        Task<bool> DeleteEmployee(int employeeId, CancellationToken cancellation = default);
        Task<PaginatedDataDto<EmployeeListDto>> GetEmployeeByFilter(EmployeeFilterDto filter, CancellationToken cancellation = default);
        Task<bool> DeleteAvatar(int employeeId, CancellationToken cancellation = default);
    }
}