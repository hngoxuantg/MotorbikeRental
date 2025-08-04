using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.PricingRepositories
{
    public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<(IEnumerable<Discount>, int)> GetFilterData(string? search, int pageNumber, int pageSize, int? categoryId, DateTime? filterStartDate, DateTime? filterEndDate, bool? isActive, CancellationToken cancellationToken = default)
        {
            Expression<Func<Discount,bool>> filter = d =>
                (string.IsNullOrWhiteSpace(search) || d.Name.Contains(search.ToLower())) &&
                (!filterStartDate.HasValue || d.StartDate > filterStartDate.Value) &&
                (!filterEndDate.HasValue || d.StartDate < filterEndDate.Value) &&
                (!isActive.HasValue || d.IsActive == isActive.Value) &&
                (!categoryId.HasValue || d.Categories.Any(dc => dc.CategoryId == categoryId.Value));

            return await GetPaged(filter,
                query => query.OrderByDescending(d => d.DiscountId),
                pageNumber,
                pageSize,
                cancellationToken,
                d => d.Categories);
        }
        public async Task<Discount?> GetDiscountById(int id, bool isTracking, CancellationToken cancellationToken = default)
        {
            IQueryable<Discount> queryable = dbContext.Discounts.Where(d => d.DiscountId == id)
                .Include(d => d.Categories)
                .ThenInclude(dc => dc.Category);
            if (!isTracking)
                queryable = queryable.AsNoTracking();
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IEnumerable<Discount>> GetExpiredDiscounts(CancellationToken cancellationToken = default)
        {
            return await dbContext.Discounts.Where(d => d.IsActive && d.EndDate < DateTime.UtcNow).ToListAsync(cancellationToken);
        }
    }
}