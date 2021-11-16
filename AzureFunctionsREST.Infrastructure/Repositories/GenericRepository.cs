using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AzureFunctionsREST.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AzureFunctionsREST.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : BaseMongoModel
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoCollection<T> collection)
        {
            this._collection = collection;
        }

        public virtual T Add(T entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public virtual IEnumerable<T> All()
        {
            return _collection.Find(_ => true).ToEnumerable();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _collection.Find(predicate).ToEnumerable();
        }

        public virtual T Get(ObjectId id)
        {
            return _collection.Find(document => document.Id.Equals(id)).FirstOrDefault();
        }

        public virtual T Update(T entity)
        {
            return _collection.FindOneAndReplace(document => document.Id.Equals(entity.Id), entity);
        }
    }
}
