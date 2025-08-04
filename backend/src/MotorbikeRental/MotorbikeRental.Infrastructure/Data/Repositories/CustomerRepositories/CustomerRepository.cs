using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Enums.ContractEnum;
using System.Linq.Expressions;

namespace MotorbikeRental.Infrastructure.Data.Repositories.CustomerRepositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Customer> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Customers.AsNoTracking()
                .Where(c => c.CustomerId == id)
                .Include(c => c.RentalContracts)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Customer not found");
        }
        public async Task<(IEnumerable<Customer>, int totalCount)> GetFilterData(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            Expression<Func<Customer, bool>> filter = c =>
                (string.IsNullOrWhiteSpace(search) ||
                c.FullName.Contains(search.ToLower()) ||
                c.IdNumber.Contains(search.ToLower()) ||
                c.PhoneNumber.Contains(search.ToLower()) ||
                c.Email.Contains(search.ToLower()));

            return await base.GetPaged(filter,
                query => query.OrderByDescending(c => c.CustomerId),
                pageNumber,
                pageSize,
                cancellationToken,
                c => c.RentalContracts);
        }
        public async Task<Customer?> GetCustomerBasicInfoById(int customerId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Customers.AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new Customer
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<bool> IsCustomerCurrentlyRenting(int customerId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId && c.RentalContracts.Any(rc => rc.RentalContractStatus == RentalContractStatus.Active))
                .AnyAsync(cancellationToken);
        }
    }
}