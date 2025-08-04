using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;

namespace MotorbikeRental.Application.Validators.UserValidators
{
    public class EmployeeValidator : IEmployeeValidator
    {
        public bool ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellation = default)
        {
            if (employeeCreateDto.DateOfBirth > DateTime.UtcNow) throw new ValidatorException("Date of birth cannot be in the future");
            return true;
        }
        public bool ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellation = default)
        {
            if (employeeUpdateDto.DateOfBirth > DateTime.UtcNow) throw new ValidatorException("Date of birth cannot be in the future");
            if (employeeUpdateDto.EmployeeId <= 0) throw new ValidatorException("Employee ID must be greater than zero");
            if (employeeUpdateDto.Salary < 0) throw new ValidatorException("Salary cannot be negative");
            return true;
        }
    }
}