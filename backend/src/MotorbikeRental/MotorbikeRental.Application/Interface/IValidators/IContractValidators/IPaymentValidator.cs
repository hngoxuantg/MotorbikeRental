using MotorbikeRental.Application.DTOs.Payments;
using MotorbikeRental.Domain.Entities.Contract;

namespace MotorbikeRental.Application.Interface.IValidators.IContractValidators
{
    public interface IPaymentValidator
    {
        bool ValidateForProcessPayment(RentalContract rentalContract, PaymentProcessDto paymentProcessDto);
    }
}
