using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class Usuario : EntidadMaestra
    {
        [FirestoreProperty]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public string Apellido { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Password { get; set; }

        [FirestoreProperty]
        public string IdPerfil { get; set; }
    }
}
