using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Validators.VehicleValidators
{
    public class MotorbikeValidator : IMotorbikeValidator
    {
        private readonly IUnitOfWork unitOfWork;
        public MotorbikeValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> ValidateForCreate(MotorbikeCreateDto motorbikeCreateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();

            if (!await unitOfWork.CategoryRepository.IsExists(nameof(Category.CategoryId), motorbikeCreateDto.CategoryId, cancellationToken))
                errors.Add("Category not found");
            if (await unitOfWork.MotorbikeRepository.IsExists(nameof(Motorbike.LicensePlate), motorbikeCreateDto.LicensePlate, cancellationToken))
                errors.Add("License plate number already exists");
            if (await unitOfWork.MotorbikeRepository.IsExists(nameof(Motorbike.ChassisNumber), motorbikeCreateDto.ChassisNumber, cancellationToken))
                errors.Add("Chassis number already exists");
            if (await unitOfWork.MotorbikeRepository.IsExists(nameof(Motorbike.EngineNumber), motorbikeCreateDto.EngineNumber, cancellationToken))
                errors.Add("Engine number already exists");

            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));

            return true;
        }

        public bool ValidateForDelete(Motorbike motorbike)
        {
            if (motorbike.Status == MotorbikeStatus.Rented)
                throw new ValidatorException("This motorbike is currently rented and cannot be deleted.");

            return true;
        }

        public async Task<bool> ValidateForUpdate(MotorbikeUpdateDto motorbikeUpdateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();

            if (!await unitOfWork.MotorbikeRepository.IsExists(nameof(Motorbike.MotorbikeId), motorbikeUpdateDto.MotorbikeId, cancellationToken))
                throw new NotFoundException("MotorBike not found");
            if (!await unitOfWork.CategoryRepository.IsExists(nameof(Category.CategoryId), motorbikeUpdateDto.CategoryId, cancellationToken))
                throw new NotFoundException("Category not found");
            if (await unitOfWork.MotorbikeRepository.IsExistsForUpdate(motorbikeUpdateDto.MotorbikeId, nameof(Motorbike.LicensePlate), motorbikeUpdateDto.LicensePlate, nameof(Motorbike.MotorbikeId), cancellationToken))
                errors.Add("License plate number already exists");
            if (await unitOfWork.MotorbikeRepository.IsExistsForUpdate(motorbikeUpdateDto.MotorbikeId, nameof(Motorbike.ChassisNumber), motorbikeUpdateDto.ChassisNumber, nameof(Motorbike.MotorbikeId), cancellationToken))
                errors.Add("Chassis number already exists");
            if (await unitOfWork.MotorbikeRepository.IsExistsForUpdate(motorbikeUpdateDto.MotorbikeId, nameof(Motorbike.EngineNumber), motorbikeUpdateDto.EngineNumber, nameof(Motorbike.MotorbikeId), cancellationToken))
                errors.Add("Engine number already exists");

            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));

            return true;
        }
    }
}