using MotorbikeRental.Application.DTOs.MaintenanceRecord;
using MotorbikeRental.Application.DTOs.Pagination;

namespace MotorbikeRental.Application.Interface.IServices.IVehicleServices
{
    public interface IMaintenanceRecordService
    {
        Task<bool> CreateMaintenanceRecord(MaintenanceRecordCreateDto maintenanceRecordCreateDto, CancellationToken cancellationToken = default);
        Task<MaintenanceRecordDto> MaintenanceRecordComplete(MaintenanceCompletionDto maintenanceCompletionDto, CancellationToken cancellationToken = default);
        Task<PaginatedDataDto<MaintenanceRecordDto>> GetMaintenanceRecordByFilter(MaintenanceRecordFilterDto maintenanceRecordFilterDto, CancellationToken cancellationToken = default);
        Task<MaintenanceRecordDto> GetById(int id, CancellationToken cancellationToken);
        Task<PaginatedDataDto<MaintenanceMotorbikeDto>> GetMotorbikesWithMaintenanceInfoByFilter(MaintenanceMotorbikeFilterDto filter, CancellationToken cancellationToken = default);
        Task<MaintenanceMotorbikeDto> GetMotorbikeMaintenanceInfoByMotorbikeId(int motorbikeId, CancellationToken cancellationToken = default);
    }
}
