using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
{
    public class MaintenanceRecordRepository : BaseRepository<MaintenanceRecord>, IMaintenanceRecordRepository
    {
        public MaintenanceRecordRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<(IEnumerable<MaintenanceRecord>, int totalCount)> GetFilterData(int pageNumber, int pageSize, bool? isCompleted, DateTime? toDate, DateTime? endDate, string? search, CancellationToken cancellationToken = default)
        {
            Expression<Func<MaintenanceRecord, bool>> filter = m =>
                (!isCompleted.HasValue || m.IsCompleted == isCompleted.Value) &&
                (!toDate.HasValue || m.MaintenanceDate > toDate.Value) &&
                (!endDate.HasValue || m.MaintenanceDate < endDate.Value) &&
                (string.IsNullOrWhiteSpace(search) ||
                 m.Motorbike.LicensePlate.ToLower().Contains(search.ToLower()) ||
                 m.Description.ToLower().Contains(search.ToLower()));

            return await GetPaged(filter,
                query => query.OrderByDescending(m => m.MaintenanceRecordId),
                pageNumber,
                pageSize,
                cancellationToken,
                m => m.Employee,
                m => m.Motorbike);
        }
        public async Task<(IEnumerable<Motorbike>, int totalCount)> GetMotorbikesWithMaintenanceRecords(int pageNumber, int pageSize, string? search, MotorbikeStatus? status, CancellationToken cancellationToken = default)
        {
            IQueryable<Motorbike> queryable = dbContext.Motorbikes
                .AsNoTracking()
                .Include(m => m.MotorbikeMaintenanceInfo);

            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                queryable = queryable.Where(m =>
                    m.LicensePlate.ToLower().Contains(lowerSearch) ||
                    m.MotorbikeName.ToLower().Contains(lowerSearch));
            }
            if (status.HasValue)
                queryable = queryable.Where(m => m.Status == status.Value);
            queryable = queryable
                .OrderBy(m => m.MotorbikeMaintenanceInfo.NextMaintenanceDate == null)
                .ThenBy(m => m.MotorbikeMaintenanceInfo.NextMaintenanceDate);
            int totalCount = await queryable.CountAsync(cancellationToken);
            queryable = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await queryable.ToListAsync(cancellationToken), totalCount);
        }
        public async Task<MaintenanceRecord?> GetMaintenanceByMotorbikeId(int motorbikeId, CancellationToken cancellationToken = default)
        {
            return await dbContext.MaintenanceRecords
                .AsNoTracking()
                .Include(m => m.Employee)
                .Include(m => m.Motorbike)
                .ThenInclude(mm => mm.MotorbikeMaintenanceInfo)
                .Where(m => m.MotorbikeId == motorbikeId)
                .OrderByDescending(m => m.MaintenanceRecordId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}