using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category?> GetByIdNoAsTracking(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetCategoriesNoTracking(CancellationToken cancellationToken = default);
        Task<string?> GetCategoryNameById(int id, CancellationToken cancellationToken = default);
        Task<Category?> GetCategoryById(int categoryId, CancellationToken cancellationToken = default);
        Task<decimal> GetDepositAmount(int motorbikeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<int>> GetAllCategoryIds(CancellationToken cancellationToken = default);
    }
}