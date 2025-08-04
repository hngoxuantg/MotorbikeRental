using System.Linq.Expressions;

namespace MotorbikeRental.Domain.Interfaces.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
        Task<T> GetById<Tid>(Tid id, CancellationToken cancellationToken = default);
        Task<T> Create(T model, CancellationToken cancellationToken = default);
        Task CreateRange(List<T> model, CancellationToken cancellationToken = default);
        Task Update(T model, CancellationToken cancellationToken = default);
        Task Delete(T models, CancellationToken cancellationToken = default);
        Task DeleteRange(List<T> models, CancellationToken cancellationToken = default);
        Task SaveChangeAsync(CancellationToken cancellationToken = default);
        Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken = default);
        Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, string idPropertyName = "Id", CancellationToken cancellationToken = default);
        Task<T?> GetWithIncludes(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes);
        Task<(IEnumerable<T>, int totalCount)> GetPaged(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 12,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[]? includes);
        void AddEntity(T entity);
        void AddRangeEntity(IEnumerable<T> entities);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);
        void DeleteRangeEntity(IEnumerable<T> entities);
    }
}
