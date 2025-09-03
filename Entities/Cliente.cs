using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class Cliente : EntidadMaestra
    {
        [FirestoreProperty]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public string Apellido { get; set; }

        [FirestoreProperty]
        public string TipoDocumento { get; set; } // "DNI", "CUIT", "CUIL"

        [FirestoreProperty]
        public string NumeroDocumento { get; set; }

        [FirestoreProperty]
        public string Direccion { get; set; }

        [FirestoreProperty]
        public string Telefono { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }
    }
}
