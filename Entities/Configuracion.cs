using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class Configuracion : EntidadMaestra
    {
        [FirestoreProperty]
        public string Clave { get; set; }

        [FirestoreProperty]
        public string Valor { get; set; }

        [FirestoreProperty]
        public string Descripcion { get; set; }
    }
}
