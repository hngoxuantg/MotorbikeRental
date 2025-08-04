using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Category?> GetByIdNoAsTracking(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.Where(c => c.CategoryId == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IEnumerable<Category>> GetCategoriesNoTracking(CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.AsNoTracking().ToListAsync(cancellationToken);
        }
        public async Task<string?> GetCategoryNameById(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.AsNoTracking().Where(c => c.CategoryId == id).Select(c => c.CategoryName).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<Category?> GetCategoryById(int categoryId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.Where(c => c.CategoryId == categoryId).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<decimal> GetDepositAmount(int motorbikeId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.AsNoTracking()
                .Where(c => c.Motorbikes.Any(m => m.MotorbikeId == motorbikeId))
                .Select(c => c.DepositAmount).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IEnumerable<int>> GetAllCategoryIds(CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.AsNoTracking().Select(c => c.CategoryId).ToListAsync(cancellationToken);
        }
    }
}