using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class UnidadMedida : EntidadMaestra
    {
        [FirestoreProperty]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public string Simbolo { get; set; }
    }
}
