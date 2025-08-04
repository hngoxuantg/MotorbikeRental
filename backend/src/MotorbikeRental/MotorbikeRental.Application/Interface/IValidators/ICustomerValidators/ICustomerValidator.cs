using MotorbikeRental.Application.DTOs.Customers;

namespace MotorbikeRental.Application.Interface.IValidators.ICustomerValidators
{
    public interface ICustomerValidator
    {
        Task<bool> ValidateForCreate(CustomerCreateDto customerCreateDto, CancellationToken cancellationToken = default);
        Task<bool> ValidateForUpdate(CustomerUpdateDto customerUpdateDto, CancellationToken cancellationToken = default);
    }
}