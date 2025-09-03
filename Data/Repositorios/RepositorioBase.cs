using Data.Interfaces;
using Entities.Interfaces;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Extensions;

namespace Data.Repositorios
{
    public abstract class RepositorioBase<T> : IRepositorio<T> where T : class, IEntity
    {
        protected readonly FirestoreDb _firestoreDb;
        protected readonly CollectionReference _collection;

        protected RepositorioBase(string collectionName)
        {
            _firestoreDb = FirestoreDb.Create("vass-net-core-pruebas");
            _collection = _firestoreDb.Collection(collectionName);
        }

        public async Task<T> Add(T entity)
        {
            var docRef = await _collection.AddAsync(entity);
            entity.Id = docRef.Id;
            return entity;
        }

        public async Task<T> Get(string id)
        {
            var snapshot = await _collection.Document(id).GetSnapshotAsync();
            return snapshot.ConvertToWithId<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var snapshot = await _collection.GetSnapshotAsync();
            return snapshot.Documents.ConvertAllToWithId<T>();
        }

        public Task Update(string id, T entity)
        {
            return _collection.Document(id).SetAsync(entity, SetOptions.MergeAll);
        }

        public Task Delete(string id)
        {
            return _collection.Document(id).DeleteAsync();
        }
    }
}
