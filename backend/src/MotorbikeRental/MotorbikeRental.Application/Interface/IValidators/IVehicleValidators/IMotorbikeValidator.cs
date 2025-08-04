using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Interface.IValidators.IVehicleValidators
{
    public interface IMotorbikeValidator
    {
        Task<bool> ValidateForCreate(MotorbikeCreateDto motorbikeCreateDto, CancellationToken cancellationToken = default);
        bool ValidateForDelete(Motorbike motorbike);
        Task<bool> ValidateForUpdate(MotorbikeUpdateDto motorbikeUpdateDto, CancellationToken cancellationToken = default);
    }
}