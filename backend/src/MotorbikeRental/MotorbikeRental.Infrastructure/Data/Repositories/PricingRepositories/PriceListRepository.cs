using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.PricingRepositories
{
    public class PriceListRepository : BaseRepository<PriceList>, IPriceListRepository
    {
        public PriceListRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<PriceList?> GetByMotorbikeId(int motorbikeId, CancellationToken cancellationToken = default)
        {
            return await dbContext.PriceLists.AsNoTracking().Where(p => p.MotorbikeId == motorbikeId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}