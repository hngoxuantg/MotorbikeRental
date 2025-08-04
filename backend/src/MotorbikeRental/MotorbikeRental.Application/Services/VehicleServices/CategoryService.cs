using AutoMapper;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using System.Threading.Tasks;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Services.VehicleServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICategoryValidator categoryValidator;
        public CategoryService(IMapper mapper, ICategoryValidator categoryValidator, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.categoryValidator = categoryValidator;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategories(CancellationToken cancellationToken = default)
        {
            IEnumerable<Category> categories = await unitOfWork.CategoryRepository.GetAll(cancellationToken);
            return mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
        public async Task<CategoryDto> CreateCategory(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            await categoryValidator.ValidateForCreate(categoryCreateDto, cancellationToken);
            Category category = mapper.Map<Category>(categoryCreateDto);
            return mapper.Map<CategoryDto>(await unitOfWork.CategoryRepository.Create(category, cancellationToken));
        }
        public async Task<CategoryDto> GetCategoryById(int id, CancellationToken cancellationToken = default)
        {
            return mapper.Map<CategoryDto>(await unitOfWork.CategoryRepository.GetCategoryById(id, cancellationToken) ?? throw new NotFoundException($"Category with id {id} not found"));
        }
        public async Task<CategoryDto> UpdateCategory(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            await categoryValidator.ValidateForUpdate(categoryUpdateDto, cancellationToken);
            Category category = mapper.Map<Category>(categoryUpdateDto);
            await unitOfWork.CategoryRepository.Update(category, cancellationToken);
            return mapper.Map<CategoryDto>(category);
        }
        public async Task<bool> DeleteCategory(int id, CancellationToken cancellationToken = default)
        {
            Category? category = await unitOfWork.CategoryRepository.GetById(id, cancellationToken) ?? throw new NotFoundException($"Category with id {id} not found");
            await categoryValidator.ValidateForDelete(category, cancellationToken);
            await unitOfWork.CategoryRepository.Delete(category, cancellationToken);
            return true;
        }
    }
}