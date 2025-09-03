using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class Perfil : EntidadMaestra
    {
        [FirestoreProperty]
        public string Nombre { get; set; }
    }
}
