using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;

namespace Data.Repositorios
{
    public class ClienteRepositorio : FirebaseRepository<Cliente>, IClienteRepositorio
    {
        public ClienteRepositorio(FirestoreDb firestoreDb) : base(firestoreDb)
        {
        }
    }
}
