using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.ContractRepositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Payment?> GetWithIncludes(int contractId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Payments.AsNoTracking().Where(p => p.ContractId == contractId)
                .Include(p => p.RentalContract)
                .ThenInclude(pr => pr.Customer)
                .Include(p =>  p.RentalContract.Incident)
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}