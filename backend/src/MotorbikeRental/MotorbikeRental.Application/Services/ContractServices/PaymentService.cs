using AutoMapper;
using MotorbikeRental.Application.DTOs.Payments;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IContractServices;
using MotorbikeRental.Application.Interface.IValidators.IContractValidators;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;

namespace MotorbikeRental.Application.Services.ContractServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IRentalContractRepository rentalContractRepository;
        private readonly IPaymentValidator paymentValidator;
        private readonly IUnitOfWork unitOfWork;
        public PaymentService(IRentalContractRepository rentalContractRepository, IPaymentValidator paymentValidator, IUnitOfWork unitOfWork)
        {
            this.rentalContractRepository = rentalContractRepository;
            this.paymentValidator = paymentValidator;
            this.unitOfWork = unitOfWork;
        }
        public async Task<PaymentPreviewDto> PreviewPayment(int contractId, CancellationToken cancellationToken = default)
        {
            RentalContract contract = await unitOfWork.RentalContractRepository.GetWithIncludes(
                c => c.ContractId == contractId,
                cancellationToken,
                c => c.Incident,
                c => c.Customer
                )
                ?? throw new NotFoundException("Contract not found");

            return MapToPaymentPreview(contract, contract.Incident, CalculateTotalAmount(contract, contract.Incident));
        }
        public async Task<PaymentDto> ProcessPayment(PaymentProcessDto paymentProcessDto, CancellationToken cancellationToken = default)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                RentalContract rentalContract = await rentalContractRepository.GetWithIncludes(
                    r => r.ContractId == paymentProcessDto.ContractId,
                    cancellationToken,
                    r => r.Customer,
                    r => r.Employee,
                    r => r.Incident
                    ) ?? throw new NotFoundException("Contract not found");

                paymentValidator.ValidateForProcessPayment(rentalContract, paymentProcessDto);
                Payment? payment = MapToPayment(paymentProcessDto, rentalContract, rentalContract.Incident, CalculateTotalAmount(rentalContract, rentalContract.Incident));
                rentalContract.IsPaid = true;
                unitOfWork.RentalContractRepository.UpdateEntity(rentalContract);
                unitOfWork.PaymentRepository.AddEntity(payment);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return MapToPaymentDto(payment, rentalContract);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        private PaymentDto MapToPaymentDto(Payment payment, RentalContract contract)
        {
            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                ContractId = contract.ContractId,
                CustomerName = contract.Customer.FullName,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentStatus = payment.PaymentStatus,
                IsIncident = contract.Incident == null ? false : true,
                ContractIndemnity = payment.ContractIndemnity ?? null,
                IncidentFineAmount = payment.IncidentFineAmount ?? null,
                EmployeeId = payment.EmployeeId,
                EmployeeName = payment.Employee?.FullName ?? "Unknown"
            };
        }
        public async Task<PaymentDto> GetById(int contractId, CancellationToken cancellationToken = default)
        {
            Payment payment = await unitOfWork.PaymentRepository.GetWithIncludes(contractId, cancellationToken)
                ?? throw new NotFoundException("Contract not found");
            return MapToPaymentDto(payment, payment.RentalContract);
        }
        private Payment MapToPayment(PaymentProcessDto paymentProcessDto, RentalContract rentalContract, Incident? incident, decimal totalAmount)
        {
            return new Payment
            {
                ContractId = rentalContract.ContractId,
                Amount = totalAmount,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = paymentProcessDto.PaymentStatus,
                ContractIndemnity = rentalContract.LateReturnFee ?? null,
                IncidentFineAmount = incident?.DamageCost ?? null,
                EmployeeId = paymentProcessDto.EmployeeId,
            };
        }

        private decimal CalculateTotalAmount(RentalContract rentalContract, Incident? incident)
        {
            return rentalContract.TotalAmount + (rentalContract.LateReturnFee ?? 0) + (incident?.DamageCost ?? 0);
        }
        private PaymentPreviewDto MapToPaymentPreview(RentalContract rentalContract, Incident? incident, decimal totalAmount)
        {
            return new PaymentPreviewDto
            {
                ContractId = rentalContract.ContractId,
                CustomerName = rentalContract.Customer.FullName,
                ContractIndemnity = rentalContract.LateReturnFee,
                IncidentFineAmount = incident?.DamageCost ?? 0,
                Amount = totalAmount
            };
        }
    }
}
