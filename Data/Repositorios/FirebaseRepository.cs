
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Entities.Interfaces;

namespace Data.Repositorios
{
    public class FirebaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly CollectionReference _collection;

        public FirebaseRepository(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
            _collection = _firestoreDb.Collection(typeof(T).Name.ToLower() + "s");
        }

        public async Task<string> Add(T entity)
        {
            DocumentReference docRef = await _collection.AddAsync(entity);
            return docRef.Id;
        }

        public async Task Delete(string id)
        {
            DocumentReference docRef = _collection.Document(id);
            await docRef.DeleteAsync();
        }

        public async Task<T> Get(string id)
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
            List<T> list = new List<T>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                T entity = document.ConvertTo<T>();
                entity.Id = document.Id;
                list.Add(entity);
            }
            return list;
        }

        public async Task Update(T entity)
        {
            DocumentReference docRef = _collection.Document(entity.Id);
            await docRef.SetAsync(entity, SetOptions.Overwrite);
        }
    }
}
