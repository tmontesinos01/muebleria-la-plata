using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;

namespace Data.Repositorios
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly CollectionReference _collection;

        public CategoriaRepositorio(FirestoreDb db)
        {
            _collection = db.Collection("categorias");
        }

        public async Task<string> Add(Categoria entity)
        {
            var doc = await _collection.AddAsync(entity);
            return doc.Id;
        }

        public async Task Delete(string id)
        {
            await _collection.Document(id).DeleteAsync();
        }

        public async Task<Categoria> Get(string id)
        {
            var doc = await _collection.Document(id).GetSnapshotAsync();
            return doc.ConvertTo<Categoria>();
        }

        public async Task<IEnumerable<Categoria>> GetAll()
        {
            var snapshot = await _collection.GetSnapshotAsync();
            return snapshot.Documents.Select(doc => doc.ConvertTo<Categoria>());
        }

        public async Task Update(Categoria entity)
        {
            await _collection.Document(entity.Id).SetAsync(entity);
        }
    }
}
