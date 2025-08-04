using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Interface.IValidators.IContractValidators
{
    public interface IContractValidator
    {
        bool ValidateForCalculateRentalPrice(Motorbike motorbike, Discount? discount, CancellationToken cancellationToken = default);
        Task<bool> ValidateForCreate(ContractCreateDto contractCreate, Motorbike motorbike, Discount? discount, decimal basePrice, decimal? discountAmount);
        bool ValidateForUpdateBeforeActivation(ContractUpdateBeforeActivationDto contractUpdate, RentalContract rentalContract, Motorbike motorbike, Discount? discount, decimal basePrice, decimal? discountAmount);
    }
}
