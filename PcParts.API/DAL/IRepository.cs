using System.Linq.Expressions;

namespace PcParts.API.DAL;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    void Insert(T obj);
    bool Delete(Guid id);
    void Update(T obj);
    Task SaveAsync();
    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[] includes
    );
}