using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class Producto : EntidadMaestra
    {
        [FirestoreProperty]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public string Descripcion { get; set; }

        [FirestoreProperty]
        public decimal Precio { get; set; }

        [FirestoreProperty]
        public decimal Stock { get; set; }

        [FirestoreProperty]
        public string IdCategoria { get; set; }

        [FirestoreProperty]
        public string ImagenUrl { get; set; }

        [FirestoreProperty]
        public string Codigo { get; set; }

        [FirestoreProperty]
        public decimal AlicuotaIva { get; set; }

        [FirestoreProperty]
        public string IdUnidadMedida { get; set; }
    }
}
