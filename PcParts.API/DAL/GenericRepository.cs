using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PcParts.API.Data;

namespace PcParts.API.DAL;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private readonly BotContext _context;
    private readonly DbSet<T> _table;
    
    public GenericRepository(BotContext context)
    {
        _context = context;
        _table = _context.Set<T>();
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }
    
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _table.FindAsync(id);
    }
    
    public void Insert(T obj)
    {
        _table.Add(obj);
    }
    
    public void Update(T obj)
    {
        _table.Attach(obj);
        _context.Entry(obj).State = EntityState.Modified;
    }
    
    public bool Delete(Guid id)
    {
        T? existing = _table.Find(id);
        if (existing != null) _table.Remove(existing);

        return existing != null;
    }
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[] includes
    )
    {
        IQueryable<T> query = _table;

        foreach (var include in includes)
            query = query.Include(include);

        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            query = orderBy(query);

        return await query.ToListAsync();
    }
}