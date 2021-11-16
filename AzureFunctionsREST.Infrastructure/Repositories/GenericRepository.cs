using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AzureFunctionsREST.Domain.Exceptions;
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
            var document = _collection.Find(document => document.Id.Equals(id)).FirstOrDefault();
            if (document == null) throw new DocumentNotFoundException();
            return document;
        }

        public virtual T Update(T entity)
        {
            var updatedDocument = _collection.FindOneAndReplace<T>(document => document.Id.Equals(entity.Id),
                                                    entity,
                                                    new FindOneAndReplaceOptions<T, T>() { ReturnDocument = ReturnDocument.After });
            if (updatedDocument == null) throw new DocumentNotFoundException();
            return updatedDocument;
        }

        public virtual T Delete(ObjectId id)
        {
            var deletedDocument = _collection.FindOneAndDelete(document => document.Id.Equals(id)); // TODO: Change to mark as deleted
            if (deletedDocument == null) throw new DocumentNotFoundException();
            return deletedDocument;
        }
    }
}
