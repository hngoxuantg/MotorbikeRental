using MotorbikeRental.Domain.Entities.Pricing;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories
{
    public interface IDiscountRepository : IBaseRepository<Discount>
    {
        Task<(IEnumerable<Discount>, int)> GetFilterData(string? search, int pageNumber, int pageSize, int? categoryId, DateTime? filterStartDate, DateTime? filterEndDate, bool? IsActive, CancellationToken cancellationToken = default);
        Task<Discount?> GetDiscountById(int id, bool isTracking, CancellationToken cancellationToken = default);
        Task<IEnumerable<Discount>> GetExpiredDiscounts(CancellationToken cancellationToken = default);
    }
}