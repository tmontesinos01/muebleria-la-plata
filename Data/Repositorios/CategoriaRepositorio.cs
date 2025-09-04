using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;

namespace Data.Repositorios
{
    public class CategoriaRepositorio : RepositorioBase<Categoria>, ICategoriaRepositorio
    {
        public CategoriaRepositorio(FirestoreDb firestoreDb) : base("categorias", firestoreDb)
        {
        }
    }
}
