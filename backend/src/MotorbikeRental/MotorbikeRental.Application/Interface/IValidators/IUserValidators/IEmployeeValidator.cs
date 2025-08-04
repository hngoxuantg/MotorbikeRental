using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Interface.IValidators.IUserValidators
{
    public interface IEmployeeValidator
    {
        bool ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default);
        bool ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default);
    }
}