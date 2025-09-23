using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Validators.UserValidators
{
    public class UserCredentialsValidator : IUserCredentialsValidator
    {
        private readonly IUnitOfWork unitOfWork;
        public UserCredentialsValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> ValidatorForCreate(UserCredentialsCreateDto userCredentialsCreateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();

            if (!await unitOfWork.EmployeeRepository.IsExists(nameof(Employee.EmployeeId), userCredentialsCreateDto.EmployeeId, cancellationToken))
                throw new NotFoundException($"Employee with id {userCredentialsCreateDto.EmployeeId} not found");
            if (await unitOfWork.UserCredentialsRepository.IsExists(nameof(UserCredentials.UserName), userCredentialsCreateDto.UserName, cancellationToken))
                errors.Add($"UserName {userCredentialsCreateDto.UserName} already exists");
            if (await unitOfWork.UserCredentialsRepository.IsExists(nameof(UserCredentials.Email), userCredentialsCreateDto.Email, cancellationToken))
                errors.Add($"Email {userCredentialsCreateDto.Email} already exists");
            if (await unitOfWork.UserCredentialsRepository.IsExists(nameof(UserCredentials.PhoneNumber), userCredentialsCreateDto.PhoneNumber, cancellationToken))
                errors.Add($"PhoneNumber {userCredentialsCreateDto.PhoneNumber} already exists");

            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));

            return true;
        }
        public async Task<bool> ValidatorForUpdate(UserCredentialsUpdateDto userCredentialsUpdateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();

            if (!await unitOfWork.EmployeeRepository.IsExists(nameof(Employee.EmployeeId), userCredentialsUpdateDto.EmployeeId, cancellationToken))
                throw new NotFoundException($"Employee with id {userCredentialsUpdateDto.EmployeeId} not found");
            if (await unitOfWork.UserCredentialsRepository.IsExistsForUpdate(userCredentialsUpdateDto.EmployeeId, nameof(UserCredentials.UserName), userCredentialsUpdateDto.UserName, nameof(UserCredentials.EmployeeId), cancellationToken))
                errors.Add($"UserName {userCredentialsUpdateDto.UserName} already exists for another employee");
            if (await unitOfWork.UserCredentialsRepository.IsExistsForUpdate(userCredentialsUpdateDto.EmployeeId, nameof(UserCredentials.Email), userCredentialsUpdateDto.Email, nameof(UserCredentials.EmployeeId), cancellationToken))
                errors.Add($"Email {userCredentialsUpdateDto.Email} already exists for another employee");
            if (await unitOfWork.UserCredentialsRepository.IsExistsForUpdate(userCredentialsUpdateDto.EmployeeId, nameof(UserCredentials.PhoneNumber), userCredentialsUpdateDto.PhoneNumber, nameof(UserCredentials.EmployeeId), cancellationToken))
                errors.Add($"PhoneNumber {userCredentialsUpdateDto.PhoneNumber} already exists for another employee");

            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));

            return true;
        }
        public async Task<bool> ValidatorForDelete(int? id, UserCredentials userCredentials, CancellationToken cancellationToken = default)
        {
            if (id == null)
                throw new NotFoundException("UserCredentials not found");
            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {id} not found");
            if (userCredentials.EmployeeId == id)
                throw new BusinessRuleException("You cannot delete yourself, please contact the administrator to delete your account.");
            if (userCredentials.RoleId == 1)
                if (await unitOfWork.UserCredentialsRepository.CountUsersInRoleAsync(userCredentials.RoleId, cancellationToken) <= 1)
                    throw new BusinessRuleException("You cannot delete the last user in this role. Please contact the administrator to delete your account.");
                
            return true;
        }
    }
}