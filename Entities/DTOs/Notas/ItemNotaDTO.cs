namespace Entities.DTOs.Notas
{
    public class ItemNotaDTO
    {
        public string id_producto { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal alicuota_iva { get; set; } = 21; // Solo 21% por ahora
        public decimal subtotal { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
    }
}
