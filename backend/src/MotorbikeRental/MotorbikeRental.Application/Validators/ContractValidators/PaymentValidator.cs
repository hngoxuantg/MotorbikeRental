using MotorbikeRental.Application.DTOs.Payments;
using MotorbikeRental.Application.Interface.IValidators.IContractValidators;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Application.Validators.ContractValidators
{
    public class PaymentValidator : IPaymentValidator
    {
        public bool ValidateForProcessPayment(RentalContract rentalContract, PaymentProcessDto paymentProcessDto)
        {
            if (rentalContract.RentalDate > paymentProcessDto.PaymentDate || rentalContract.ActualReturnDate > paymentProcessDto.PaymentDate)
                throw new BusinessRuleException("Payment date must be within the rental period of the contract.");
            if (rentalContract.IsPaid)
                throw new BusinessRuleException("Contract has already been paid.");
            if (rentalContract.RentalContractStatus != RentalContractStatus.Completed)
                throw new BusinessRuleException("Contract must be completed before processing payment.");
            return true;
        }
    }
}
