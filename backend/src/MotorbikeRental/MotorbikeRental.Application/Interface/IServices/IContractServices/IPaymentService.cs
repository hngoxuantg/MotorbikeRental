using MotorbikeRental.Application.DTOs.Payments;

namespace MotorbikeRental.Application.Interface.IServices.IContractServices
{
    public interface IPaymentService
    {
        Task<PaymentPreviewDto> PreviewPayment(int contractId, CancellationToken cancellationToken = default);
        Task<PaymentDto> ProcessPayment(PaymentProcessDto paymentProcessDto, CancellationToken cancellationToken = default);
        Task<PaymentDto> GetById(int contractId, CancellationToken cancellationToken = default);
    }
}
