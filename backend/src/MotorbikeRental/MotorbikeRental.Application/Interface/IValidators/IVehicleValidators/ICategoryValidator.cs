using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Interface.IValidators.IVehicleValidators
{
    public interface ICategoryValidator
    {
        Task<bool> ValidateForCreate(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default);
        Task<bool> ValidateForDelete(Category category, CancellationToken cancellationToken = default);
        Task<bool> ValidateForUpdate(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default);
        Task<bool> ValidateForGet(int id, CancellationToken cancellationToken = default);
    }
}