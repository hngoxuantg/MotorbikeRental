using MotorbikeRental.Application.DTOs.Discount;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IValidators.IDiscountValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Validators.DiscountValidators
{
    public class DiscountValidator : IDiscountValidator
    {
        private readonly IUnitOfWork unitOfWork;
        public DiscountValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<(bool, List<string>)> ValidateForCreate(DiscountCreateDto discountCreateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();
            List<Category> category = (await unitOfWork.CategoryRepository.GetCategoriesNoTracking(cancellationToken)).ToList();
            List<int> categoryIds = category.Select(e => e.CategoryId).ToList();
            if (discountCreateDto.StartDate > discountCreateDto.EndDate)
                errors.Add("Start date cannot be later than end date.");
            if (discountCreateDto.CategoryId.Distinct().Count() < discountCreateDto.CategoryId.Count())
                errors.Add("Duplicate category IDs are not allowed.");
            for (int i = 0; i < discountCreateDto.CategoryId.Count; i++)
                if (!categoryIds.Contains(discountCreateDto.CategoryId[i]))
                    errors.Add($"Category ID {discountCreateDto.CategoryId[i]} does not exist.");
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));
            return (true, category.Where(e => discountCreateDto.CategoryId.Contains(e.CategoryId)).Select(e => e.CategoryName).ToList());
        }
        public async Task<(bool, List<string>)> ValidateForUpdate(DiscountUpdateDto discountUpdateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();
            List<Category> categories = (await unitOfWork.CategoryRepository.GetCategoriesNoTracking(cancellationToken)).ToList();
            List<int> categoryIds = categories.Select(e => e.CategoryId).ToList();
            if (discountUpdateDto.StartDate > discountUpdateDto.EndDate)
                errors.Add("Start date cannot be later than end date.");
            if (discountUpdateDto.CategoryId.Distinct().Count() < discountUpdateDto.CategoryId.Count())
                errors.Add("Duplicate category IDs are not allowed.");
            for (int i = 0; i < discountUpdateDto.CategoryId.Count; i++)
                if (!categoryIds.Contains(discountUpdateDto.CategoryId[i]))
                    errors.Add($"Category ID {discountUpdateDto.CategoryId[i]} does not exist.");
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));
            return (true, categories.Where(e => discountUpdateDto.CategoryId.Contains(e.CategoryId)).Select(e => e.CategoryName).ToList());
        }
    }
}
