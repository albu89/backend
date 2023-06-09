using System.Linq.Expressions;
namespace CE_API_V2.Data.Repositories.Interfaces;

public interface IGenericRepository<T>
{

    T Insert(T data);
    T GetById(string id);
    IEnumerable<T> Find(Expression<Func<T, bool>> func);
}