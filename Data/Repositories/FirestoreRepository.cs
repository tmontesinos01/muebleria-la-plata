using Google.Cloud.Firestore;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using System.Linq;
using Entities.Interfaces;

namespace Data.Repositories
{
    public class FirestoreRepository<T> : IRepository<T> where T : EntidadMaestra, IEntity
    {
        private readonly CollectionReference _collection;

        public FirestoreRepository(FirestoreDb firestoreDb)
        {
            _collection = firestoreDb.Collection(typeof(T).Name.ToLower() + "s");
        }

        public async Task<T?> Get(string id)
        {
            DocumentReference docRef = _collection.Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                T entity = snapshot.ConvertTo<T>();
                entity.Id = snapshot.Id;
                return entity;
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            QuerySnapshot snapshot = await _collection.GetSnapshotAsync();
            return snapshot.Documents.Select(doc =>
            {
                T entity = doc.ConvertTo<T>();
                entity.Id = doc.Id;
                return entity;
            });
        }

        public async Task<string> Add(T entity)
        {
            DocumentReference docRef = await _collection.AddAsync(entity);
            return docRef.Id;
        }

        public async Task Update(T entity)
        {
            DocumentReference docRef = _collection.Document(entity.Id);
            await docRef.SetAsync(entity, SetOptions.MergeAll);
        }

        public async Task Delete(string id)
        {
            DocumentReference docRef = _collection.Document(id);
            await docRef.DeleteAsync();
        }
    }
}
