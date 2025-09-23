using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IValidators.IContractValidators;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.ContractEnum;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Validators.ContractValidators
{
    public class ContractValidator : IContractValidator
    {
        private readonly IUnitOfWork unitOfWork;
        public ContractValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public bool ValidateForCalculateRentalPrice(Motorbike motorbike, Discount? discount, CancellationToken cancellationToken = default)
        {
            if (discount != null)
            {
                if (!discount.IsActive)
                    throw new BusinessRuleException("Discount is not active");
                if (!discount.Categories.Any(c => c.CategoryId == motorbike.CategoryId))
                    throw new BusinessRuleException($"Motorbike category {motorbike.CategoryId} is not eligible for the discount {discount.DiscountId}");
            }

            if (motorbike.Status != MotorbikeStatus.Available)
                throw new BusinessRuleException($"Motorbike with id {motorbike.MotorbikeId} is not available for rental");

            return true;
        }
        public async Task<bool> ValidateForCreate(ContractCreateDto contractCreate, Motorbike motorbike, Discount? discount, decimal basePrice, decimal? discountAmount)
        {
            ValidateForCalculateRentalPrice(motorbike, discount);

            if (!contractCreate.IdCardHeld && contractCreate.Status != RentalContractStatus.Pending)
                throw new BusinessRuleException("ID card can only be held when the contract is pending");

            if (contractCreate.IdCardHeld && contractCreate.Status != RentalContractStatus.Active)
                throw new BusinessRuleException("ID card can only be held when the contract is active, not pending");

            if (contractCreate.RentalDate.AddMinutes(30) < DateTime.UtcNow)
                throw new BusinessRuleException("Rental date cannot be in the past");

            if (await unitOfWork.CustomerRepository.IsCustomerCurrentlyRenting(contractCreate.CustomerId))
                throw new BusinessRuleException("Customer is currently renting a motorbike and cannot create a new contract");

            if (contractCreate.Status == RentalContractStatus.Pending && contractCreate.IdCardHeld)
                throw new BusinessRuleException("ID card can only be held when the contract is active, not pending");

            if (contractCreate.ExpectedReturnDate < contractCreate.RentalDate)
                throw new BusinessRuleException("Return date must be greater than rental date");

            if (contractCreate.TotalAmount > (basePrice - (discountAmount != null ? discountAmount : 0)))
                throw new BusinessRuleException("Total amount cannot be greater than calculated rental price");

            if (contractCreate.Status != RentalContractStatus.Active && contractCreate.Status != RentalContractStatus.Pending)
                throw new BusinessRuleException("Invalid contract status. Only Active or Pending are allowed.");

            if (contractCreate.RentalTypeStatus == RentalTypeStatus.Hourly)
            {
                if (contractCreate.ExpectedReturnDate < contractCreate.RentalDate.AddHours(1))
                    throw new BusinessRuleException("For hourly rentals, the expected return date must be at least 1 hour after the rental date");
            }
            else if (contractCreate.RentalTypeStatus == RentalTypeStatus.Daily)
            {
                if (contractCreate.ExpectedReturnDate < contractCreate.RentalDate.AddDays(1))
                    throw new BusinessRuleException("For daily rentals, the expected return date must be at least 1 day after the rental date");
            }

            if (contractCreate.RentalDate > DateTime.UtcNow.AddHours(1) && contractCreate.Status != RentalContractStatus.Pending)
                throw new BusinessRuleException("Rental date cannot be in the future unless the contract is pending");
            return true;
        }
        public bool ValidateForUpdateBeforeActivation(ContractUpdateBeforeActivationDto contractUpdate, RentalContract rentalContract, Motorbike motorbike, Discount? discount, decimal basePrice, decimal? discountAmount)
        {
            if (discount != null)
            {
                if (!discount.IsActive)
                    throw new BusinessRuleException("Discount is not active");
                if (!discount.Categories.Any(c => c.CategoryId == motorbike.CategoryId))
                    throw new BusinessRuleException($"Motorbike category {motorbike.CategoryId} is not eligible for the discount {discount.DiscountId}");
            }

            if (motorbike.Status != MotorbikeStatus.Reserved)
                throw new BusinessRuleException($"Motorbike with id {motorbike.MotorbikeId} is not available for rental");

            if (contractUpdate.RentalDate < DateTime.UtcNow)
                throw new BusinessRuleException("Rental date cannot be in the past");

            if (contractUpdate.ExpectedReturnDate < contractUpdate.RentalDate)
                throw new BusinessRuleException("Return date must be greater than rental date");

            if (contractUpdate.TotalAmount > (basePrice - (discountAmount != null ? discountAmount : 0)))
                throw new BusinessRuleException("Total amount cannot be greater than calculated rental price");

            if (rentalContract.RentalContractStatus != RentalContractStatus.Pending)
                throw new BusinessRuleException("Invalid contract status. Only Pending is allowed for updates before activation.");

            if (contractUpdate.RentalTypeStatus == RentalTypeStatus.Hourly)
            {
                if (contractUpdate.ExpectedReturnDate < contractUpdate.RentalDate.AddHours(1))
                    throw new BusinessRuleException("For hourly rentals, the expected return date must be at least 1 hour after the rental date");
            }
            else if (contractUpdate.RentalTypeStatus == RentalTypeStatus.Daily)
            {
                if (contractUpdate.ExpectedReturnDate < contractUpdate.RentalDate.AddDays(1))
                    throw new BusinessRuleException("For daily rentals, the expected return date must be at least 1 day after the rental date");
            }
            return true;
        }
    }
}
