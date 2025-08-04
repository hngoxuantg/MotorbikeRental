using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq.Expressions;

namespace MotorbikeRental.Infrastructure.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MotorbikeRentalDbContext dbContext;
        public BaseRepository(MotorbikeRentalDbContext motorbikeRentalDbContext)
        {
            dbContext = motorbikeRentalDbContext;
        }

        public virtual async Task<T> Create(T model, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().Add(model);
            await dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public virtual async Task CreateRange(List<T> models, CancellationToken cancellationToken = default)
        {
            dbContext.AddRange(models);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task Delete(T model, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().Remove(model);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteRange(List<T> models, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().RemoveRange(models);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetById<Tid>(Tid id, CancellationToken cancellationToken = default)
        {
            T? model = await dbContext.Set<T>().FindAsync(id, cancellationToken);
            return model != null ? model : throw new NotFoundException("No data");
        }
        public virtual async Task SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task Update(T model, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().Update(model);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);
            return await dbContext.Set<T>().AsNoTracking().AnyAsync(lambda, cancellationToken);
        }
        public virtual async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, string idPropertyName = "Id", CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);

            var idProperty = Expression.Property(parameter, idPropertyName);
            var idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

            var combinedExpression = Expression.AndAlso(equality, idEquality);
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return await dbContext.Set<T>().AsNoTracking().AnyAsync(lambda, cancellationToken);
        }
        public virtual async Task<T?> GetWithIncludes(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> queryable = dbContext.Set<T>().AsNoTracking().Where(expression);
            if (includes != null && includes.Length > 0)
                for (int i = 0; i < includes.Length; i++)
                    queryable = queryable.Include(includes[i]);
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<(IEnumerable<T>, int totalCount)> GetPaged(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 12,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = dbContext.Set<T>().AsNoTracking();
            if (filter != null)
                query = query.Where(filter);
            if(includes != null && includes.Length > 0)
                for(int i = 0; i < includes.Length; i++)
                    query = query.Include(includes[i]);
            int count = await query.CountAsync(cancellationToken);
            if(orderBy != null)
                query = orderBy(query);
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await query.ToListAsync(cancellationToken), count);
        }

        public virtual void AddEntity(T entity)
        {
            dbContext.Set<T>().Add(entity);
        }
        public virtual void AddRangeEntity(IEnumerable<T> entities)
        {
            dbContext.Set<T>().AddRange(entities);
        }
        public virtual void UpdateEntity(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public virtual void DeleteEntity(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }
        public virtual void DeleteRangeEntity(IEnumerable<T> entities)
        {
            dbContext.Set<T>().RemoveRange(entities);
        }
    }
}