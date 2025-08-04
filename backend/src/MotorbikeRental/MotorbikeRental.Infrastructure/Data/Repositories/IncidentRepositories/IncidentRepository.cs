using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Infrastructure.Data.Contexts;
using MotorbikeRental.Domain.Enums.IncidentEnum;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using System.Linq.Expressions;

namespace MotorbikeRental.Infrastructure.Data.Repositories.IncidentRepositories
{
    public class IncidentRepository : BaseRepository<Incident>, IIncidentRepository
    {
        public IncidentRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Incident?> GetIncidentByIdWithIncludes(int incidentId, bool isTracking, CancellationToken cancellationToken = default)
        {
            IQueryable<Incident> queryable = dbContext.Incidents.Where(i => i.IncidentId == incidentId)
                .Include(i => i.Images);
            if (!isTracking)
                queryable = queryable.AsNoTracking();
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<bool> IsExistsContractIdForUpdate(int incidentId, int contractId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Incidents.AnyAsync(i => i.ContractId == contractId && i.IncidentId != incidentId, cancellationToken);
        }
        public async Task<(IEnumerable<Incident>, int totalCount)> GetIncidentsByFilter(int pageNumber, int pageSize, MotorbikeStatus? status, DateTime? fromDate, DateTime? toDate, bool? isResolved, CancellationToken cancellationToken = default)
        {
            Expression<Func<Incident, bool>> filter = i =>
                (!fromDate.HasValue || i.IncidentDate > fromDate.Value) &&
                (!toDate.HasValue || i.IncidentDate < toDate.Value) &&
                (!isResolved.HasValue || i.IsResolved == isResolved.Value) &&
                (!status.HasValue || i.Motorbike.Status == status.Value);

            return await GetPaged(filter,
                query => query.OrderByDescending(i => i.IncidentId),
                pageNumber,
                pageSize,
                cancellationToken,
                i => i.Images);
        }
    }
}