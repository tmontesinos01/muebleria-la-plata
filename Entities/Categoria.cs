using Google.Cloud.Firestore;
using Entities.Interfaces;

namespace Entities
{
    [FirestoreData]
    public class Categoria : IEntity
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? Nombre { get; set; }
    }
}
