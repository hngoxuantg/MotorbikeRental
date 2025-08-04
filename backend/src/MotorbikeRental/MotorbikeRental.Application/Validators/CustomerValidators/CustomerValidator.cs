using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IValidators.ICustomerValidators;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;

namespace MotorbikeRental.Application.Validators.CustomerValidators
{
    public class CustomerValidator : ICustomerValidator
    {
        private readonly IUnitOfWork unitOfWork;
        public CustomerValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> ValidateForCreate(CustomerCreateDto customerCreateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();
            if (await unitOfWork.CustomerRepository.IsExists(nameof(Customer.IdNumber), customerCreateDto.IdNumber, cancellationToken))
                errors.Add("Id number already exists");
            if (await unitOfWork.CustomerRepository.IsExists(nameof(Customer.PhoneNumber), customerCreateDto.PhoneNumber, cancellationToken))
                errors.Add("Phone number already exists");
            if (await unitOfWork.CustomerRepository.IsExists(nameof(Customer.Email), customerCreateDto.Email, cancellationToken))
                errors.Add("Email already exists");
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));
            return true;
        }

        public async Task<bool> ValidateForUpdate(CustomerUpdateDto customerUpdateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();
            if (!await unitOfWork.CustomerRepository.IsExists(nameof(Customer.CustomerId), customerUpdateDto.CustomerId, cancellationToken))
                throw new NotFoundException("Customer not found");
            if (await unitOfWork.CustomerRepository.IsExistsForUpdate(customerUpdateDto.CustomerId, nameof(Customer.IdNumber), customerUpdateDto.IdNumber, nameof(Customer.CustomerId), cancellationToken))
                errors.Add("Id number already exists");
            if (await unitOfWork.CustomerRepository.IsExistsForUpdate(customerUpdateDto.CustomerId, nameof(Customer.PhoneNumber), customerUpdateDto.PhoneNumber, nameof(Customer.CustomerId), cancellationToken))
                errors.Add("Phone number already exists");
            if (await unitOfWork.CustomerRepository.IsExistsForUpdate(customerUpdateDto.CustomerId, nameof(Customer.Email), customerUpdateDto.Email, nameof(Customer.CustomerId), cancellationToken))
                errors.Add("Email already exists");
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));
            return true;
        }
    }
}