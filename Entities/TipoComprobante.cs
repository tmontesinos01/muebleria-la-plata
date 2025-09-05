using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class TipoComprobante : EntidadMaestra
    {
        [FirestoreProperty]
        public string Codigo { get; set; } // Código único para identificar el tipo de comprobante

        [FirestoreProperty]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public string Abreviatura { get; set; }

        // Por ejemplo, para saber si suma o resta en los movimientos de cuenta
        [FirestoreProperty]
        public int Signo { get; set; } = 1; 
    }
}
