using MotorbikeRental.Application.DTOs.Discount;

namespace MotorbikeRental.Application.Interface.IValidators.IDiscountValidators
{
    public interface IDiscountValidator
    {
        Task<(bool, List<string>)> ValidateForCreate(DiscountCreateDto discountCreateDto, CancellationToken cancellationToken = default);
        Task<(bool, List<string>)> ValidateForUpdate(DiscountUpdateDto discountUpdateDto, CancellationToken cancellationToken = default);
    }
}
