using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Enums.ContractEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.ContractRepositories
{
    public class RentalContractRepository : BaseRepository<RentalContract>, IRentalContractRepository
    {
        public RentalContractRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<RentalContract?> GetContractById(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.RentalContracts
                .AsNoTracking()
                .Where(r => r.ContractId == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<RentalContract> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.RentalContracts
                .Include(r => r.Customer)
                .Include(r => r.Motorbike)
                .Include(r => r.Discount)
                .Include(r => r.Employee)
                .Include(r => r.Incident)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ContractId == id, cancellationToken) ?? throw new KeyNotFoundException($"Rental contract with ID {id} not found.");
        }
        public async Task<(IEnumerable<RentalContract>, int totalCount)> GetFilterData(string? search, int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, RentalContractStatus? status, CancellationToken cancellation = default)
        {
            Expression<Func<RentalContract, bool>> filter = r =>
                (string.IsNullOrWhiteSpace(search) || r.Customer.FullName.Contains(search.ToLower()) ||
                r.Motorbike.MotorbikeName.Contains(search.ToLower()) ||
                r.Motorbike.LicensePlate.Contains(search.ToLower()) ||
                r.Employee.FullName.Contains(search.ToLower())) &&
               (!fromDate.HasValue || r.RentalDate >= fromDate.Value) &&
               (!toDate.HasValue || r.RentalDate <= toDate.Value) &&
               (!status.HasValue || r.RentalContractStatus == status.Value);

            return await GetPaged(filter,
                query => query.OrderByDescending(r => r.ContractId),
                pageNumber,
                pageSize,
                cancellation,
                r => r.Customer,
                r => r.Motorbike,
                r => r.Discount);
        }
    }
}