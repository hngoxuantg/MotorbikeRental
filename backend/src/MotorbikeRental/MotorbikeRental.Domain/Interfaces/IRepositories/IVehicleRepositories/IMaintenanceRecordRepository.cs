using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories
{
    public interface IMaintenanceRecordRepository : IBaseRepository<MaintenanceRecord>
    {
        Task<(IEnumerable<MaintenanceRecord>, int totalCount)> GetFilterData(int pageNumber, int pageSize, bool? isCompleted, DateTime? toDate, DateTime? endDate, string? search, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Motorbike>, int totalCount)> GetMotorbikesWithMaintenanceRecords(int pageNumber, int pageSize, string? search, MotorbikeStatus? status, CancellationToken cancellationToken = default);
        Task<MaintenanceRecord?> GetMaintenanceByMotorbikeId(int motorbikeId, CancellationToken cancellationToken = default);
    }
}