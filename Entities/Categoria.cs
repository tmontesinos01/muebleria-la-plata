using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class Categoria : EntidadMaestra
    {
        [FirestoreProperty]
        public string Nombre { get; set; }
    }
}
