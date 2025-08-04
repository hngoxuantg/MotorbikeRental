using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories
{
    public interface IRentalContractRepository : IBaseRepository<RentalContract>
    {
        Task<RentalContract?> GetContractById(int id, CancellationToken cancellationToken = default);
        Task<(IEnumerable<RentalContract>, int totalCount)> GetFilterData(string? search, int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, RentalContractStatus? status, CancellationToken cancellation = default);
        Task<RentalContract> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default);
    }
}