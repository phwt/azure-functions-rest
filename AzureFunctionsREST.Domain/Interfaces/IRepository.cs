using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AzureFunctionsREST.Domain.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T Get(ObjectId id);
        IEnumerable<T> All();
        IEnumerable<T> Find(FilterDefinition<T> filter);
        T Delete(ObjectId id);
    }
}
