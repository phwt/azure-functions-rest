using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace AzureFunctionsREST.Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T Get(ObjectId id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
