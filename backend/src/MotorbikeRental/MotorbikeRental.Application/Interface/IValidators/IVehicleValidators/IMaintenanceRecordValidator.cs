using MotorbikeRental.Application.DTOs.MaintenanceRecord;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Interface.IValidators.IVehicleValidators
{
    public interface IMaintenanceRecordValidator
    {
        Task<bool> ValidateForCreate(Motorbike motorbike, MaintenanceRecordCreateDto maintenanceRecordCreateDto, CancellationToken cancellationToken = default);
        bool ValidateForComplete(MaintenanceRecord maintenanceRecord, MaintenanceCompletionDto maintenanceCompletionDto);
    }
}
