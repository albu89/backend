﻿using System.Linq.Expressions;
namespace CE_API_V2.Data.Repositories.Interfaces;

public interface IGenericRepository<T>
{

    T Insert(T data);
    T GetById(string id);
    T GetByGuid(Guid id);
    IEnumerable<T> Get( Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "");
}