using MotorbikeRental.Domain.Entities.Contract;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetWithIncludes(int contractId, CancellationToken cancellationToken = default);
    }
}