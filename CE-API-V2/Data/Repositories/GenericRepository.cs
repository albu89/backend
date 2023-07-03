using System.Linq.Expressions;
using CE_API_V2.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CE_API_V2.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly CEContext _context;
    internal DbSet<T> dbSet;
    public GenericRepository(CEContext context)
    {
        _context = context;
        dbSet = context.Set<T>();
    }

    public T Insert(T data)
    {
        _context.Set<T>().Add(data);
        _context.SaveChanges();
        return data;
    }

    public T GetById(string id)
    {
        return dbSet.Find(id);
    }
    
    public T GetByGuid(Guid id)
    {
        return dbSet.Find(id);
    }
    
    public IEnumerable<T> Find(Expression<Func<T, bool>> func)
    {
        return dbSet.Where(func);
    }
    
    public virtual IEnumerable<T> Get(
        Expression<Func<T, bool>> filter = default,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = default,
        string includeProperties = "")
    {
        IQueryable<T> query = dbSet;

        if (filter != default)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != default)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }
}