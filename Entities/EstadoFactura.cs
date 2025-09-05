using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class EstadoFactura : EntidadMaestra
    {
        [FirestoreProperty]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public string Descripcion { get; set; }

        [FirestoreProperty]
        public string Codigo { get; set; } // Código único para identificar el estado

        [FirestoreProperty]
        public bool PermiteAnulacion { get; set; } = false; // Si este estado permite anulación

        [FirestoreProperty]
        public bool EsEstadoFinal { get; set; } = false; // Si es un estado final (no se puede cambiar)
    }
}
