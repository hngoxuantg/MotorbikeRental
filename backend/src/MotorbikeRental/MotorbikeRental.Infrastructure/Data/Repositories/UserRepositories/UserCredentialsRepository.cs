using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.UserRepositories
{
    public class UserCredentialsRepository : BaseRepository<UserCredentials>, IUserCredentialsRepository
    {
        public UserCredentialsRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<bool> IsExistsForUpdate<Tvalue, Tid>(string key, Tvalue value, string entity, string idPropertyName, Tid id, CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(UserCredentials), "x");

            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);

            var idObjectProperty = Expression.Property(parameter, entity);
            var idProperty = Expression.Property(idObjectProperty, idPropertyName);
            var idConstant = Expression.Constant(id);
            var idEquality = Expression.NotEqual(idProperty, idConstant);

            var combinedExpression = Expression.AndAlso(equality, idEquality);
            var lambda = Expression.Lambda<Func<UserCredentials, bool>>(combinedExpression, parameter);
            return await dbContext.UserCredentials.AsNoTracking().AnyAsync(lambda, cancellationToken);
        }
        public async Task<UserCredentials> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.UserCredentials.Where(u => u.Id == id)
                .AsNoTracking()
                .Include(u => u.Employee)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("UserCredentials not found");
        }
        public async Task<UserCredentials> GetByEmployeeId(int employeeId, bool isNoTracking, CancellationToken cancellationToken = default)
        {
            IQueryable<UserCredentials> queryable = dbContext.UserCredentials.Where(u => u.EmployeeId == employeeId)
                .Include(u => u.Employee)
                .Include(u => u.Roles);
            if (!isNoTracking)
                queryable = queryable.AsNoTracking();
            return await queryable.FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"UserCredentials with employeeId {employeeId} not found");
        }
        public async Task<UserCredentials?> GetByUserNameInCludes(string userName, CancellationToken cancellationToken = default)
        {
            return await dbContext.UserCredentials.AsNoTracking()
                .Where(u => u.UserName == userName)
                .Include(u => u.Employee)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<int> CountUsersInRoleAsync(int? roleId, CancellationToken cancellationToken = default)
        {
            return await dbContext.UserCredentials.CountAsync(u => u.RoleId == roleId, cancellationToken);
        }
    }
}
