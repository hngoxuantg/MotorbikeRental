using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq.Expressions;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
{
    public class MotorbikeRepository : BaseRepository<Motorbike>, IMotorbikeRepository
    {
        public MotorbikeRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Motorbike> GetByIdNoAsTracking(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Motorbikes.Where(c => c.MotorbikeId == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Category not found");
        }
        public async Task<Motorbike> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Motorbikes.Where(m => m.MotorbikeId == id)
                .AsNoTracking()
                .Include(m => m.Category)
                .Include(m => m.PriceList)
                .Include(m => m.Incidents)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Motorbike not found");
        }
        public async Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(int? categoryId, string? brand, string? search, MotorbikeStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            Expression<Func<Motorbike, bool>> filter = m =>
                (!categoryId.HasValue || m.CategoryId == categoryId) &&
                (!status.HasValue || m.Status == status) &&
                (string.IsNullOrEmpty(brand) || m.Brand == brand) &&
                (string.IsNullOrEmpty(search) || m.MotorbikeName.Contains(search.ToLower()) || m.Category.CategoryName.Contains(search.ToLower()) || m.Brand.Contains(search.ToLower()) || m.LicensePlate.Contains(search.ToLower()));

            return await GetPaged(filter,
                query => query.OrderByDescending(m => m.MotorbikeId),
                pageNumber,
                pageSize,
                cancellationToken,
                m => m.Category,
                m => m.PriceList
                );
        }
        public async Task<IEnumerable<string>> GetDistinctBrands(CancellationToken cancellationToken = default)
        {
            return await dbContext.Motorbikes
                .Where(m => !string.IsNullOrEmpty(m.Brand))
                .Select(m => m.Brand)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
        public async Task<Motorbike?> GetMotorbikeBasicInfoById(int motorbikeId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Motorbikes.AsNoTracking()
                .Where(m => m.MotorbikeId == motorbikeId)
                .Select(m => new Motorbike
                {
                    MotorbikeId = m.MotorbikeId,
                    MotorbikeName = m.MotorbikeName,
                    LicensePlate = m.LicensePlate,
                    ImageUrl = m.ImageUrl,
                })
                .FirstOrDefaultAsync(cancellationToken);

        }
    }
}