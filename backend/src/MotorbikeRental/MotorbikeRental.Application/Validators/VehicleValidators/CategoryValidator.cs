using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Validators.VehicleValidators
{
    public class CategoryValidator : ICategoryValidator
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> ValidateForCreate(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            if (await unitOfWork.CategoryRepository.IsExists(nameof(Category.CategoryName), categoryCreateDto.CategoryName, cancellationToken))
                throw new ValidatorException("Category name already exists");
            return true;
        }

        public async Task<bool> ValidateForDelete(Category category, CancellationToken cancellationToken = default)
        {
            if (await unitOfWork.MotorbikeRepository.IsExists(nameof(Motorbike.CategoryId), category.CategoryId, cancellationToken))
                throw new ValidatorException("Cannot delete category because it is associated with existing motorbikes");
            return true;
        }

        public async Task<bool> ValidateForGet(int id, CancellationToken cancellationToken = default)
        {
            if (!await unitOfWork.CategoryRepository.IsExists(nameof(Category.CategoryId), id, cancellationToken))
                throw new ValidatorException("Category ID not found");
            return true;
        }

        public async Task<bool> ValidateForUpdate(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            if (!await unitOfWork.CategoryRepository.IsExists(nameof(Category.CategoryId), categoryUpdateDto.CategoryId, cancellationToken))
                throw new ValidatorException($"Entity with ID: {categoryUpdateDto.CategoryId} not found");
            if (await unitOfWork.CategoryRepository.IsExistsForUpdate(categoryUpdateDto.CategoryId, nameof(Category.CategoryName), categoryUpdateDto.CategoryName, nameof(Category.CategoryId)))
                throw new ValidatorException($"Category name already exists");
            return true;
        }
    }
}