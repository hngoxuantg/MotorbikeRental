using MotorbikeRental.Domain.Entities.Pricing;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories
{
    public interface IPriceListRepository : IBaseRepository<PriceList>
    {
        Task<PriceList?> GetByMotorbikeId(int motorbikeId, CancellationToken cancellationToken = default);
    }
}