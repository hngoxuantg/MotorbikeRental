using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Enums.IncidentEnum;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents
{
    public interface IIncidentRepository : IBaseRepository<Incident>
    {
        Task<Incident?> GetIncidentByIdWithIncludes(int incidentId, bool isTracking, CancellationToken cancellationToken = default);
        Task<bool> IsExistsContractIdForUpdate(int incidentId, int contractId, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Incident>, int totalCount)> GetIncidentsByFilter(int pageNumber, int pageSize, MotorbikeStatus? status, DateTime? fromDate, DateTime? toDate, bool? isResolved, CancellationToken cancellationToken = default);
    }
}