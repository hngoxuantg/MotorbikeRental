using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Interface.IServices.IVehicleServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories(CancellationToken cancellationToken = default);
        Task<CategoryDto> CreateCategory(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default);
        Task<CategoryDto> GetCategoryById(int id, CancellationToken cancellationToken = default);
        Task<CategoryDto> UpdateCategory(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteCategory(int id, CancellationToken cancellationToken = default);
    }
}