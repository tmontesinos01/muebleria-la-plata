using Google.Cloud.Firestore;
using Entities.Interfaces;

namespace Entities
{
    [FirestoreData]
    public class Cliente : IEntity
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? Nombre { get; set; }

        [FirestoreProperty]
        public string? Apellido { get; set; }

        [FirestoreProperty]
        public string? Email { get; set; }

        [FirestoreProperty]
        public string? Telefono { get; set; }
    }
}
