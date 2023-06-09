using System.Linq.Expressions;
using CE_API_V2.Data.Repositories.Interfaces;
namespace CE_API_V2.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
{
    public GenericRepository(CEContext context)
    {
        throw new NotImplementedException();
    }

    public T Insert(T data)
    {
        throw new NotImplementedException();
    }
    public T GetById(string id)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<T> Find(Expression<Func<T, bool>> func)
    {
        throw new NotImplementedException();
    }
}