using MotorbikeRental.Application.DTOs.MaintenanceRecord;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Application.Exceptions;

namespace MotorbikeRental.Application.Validators.VehicleValidators
{
    public class MaintenanceRecordValidator : IMaintenanceRecordValidator
    {
        private readonly IEmployeeRepository employeeRepository;
        public MaintenanceRecordValidator(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public async Task<bool> ValidateForCreate(Motorbike motorbike, MaintenanceRecordCreateDto maintenanceRecordCreateDto, CancellationToken cancellationToken = default)
        {
            if (motorbike.Status == MotorbikeStatus.Rented || motorbike.Status == MotorbikeStatus.OutOfService || motorbike.Status == MotorbikeStatus.Reserved)
                throw new BusinessRuleException("Motorbike is not available for maintenance. It must be in a valid state (Available or In Service).");
            if (maintenanceRecordCreateDto.MaintenanceDate < DateTime.UtcNow.Date)
                throw new ValidatorException("Maintenance date cannot be in the past.");
            if (!await employeeRepository.IsExists("EmployeeId", maintenanceRecordCreateDto.EmployeeId, cancellationToken))
                throw new NotFoundException($"Employee with ID {maintenanceRecordCreateDto.EmployeeId} not found.");

            return true;
        }
        public bool ValidateForComplete(MaintenanceRecord maintenanceRecord, MaintenanceCompletionDto maintenanceCompletionDto)
        {
            List<string> errors = new List<string>();

            if(maintenanceRecord.IsCompleted)
                errors.Add("Maintenance record is already completed.");
            if(maintenanceCompletionDto.NextMaintenanceDate <= maintenanceRecord.MaintenanceDate)
                errors.Add("Next maintenance date must be after the current maintenance date.");

            if(errors.Any())
                throw new ValidatorException(string.Join("; ", errors));

            return true;
        }
    }
}
