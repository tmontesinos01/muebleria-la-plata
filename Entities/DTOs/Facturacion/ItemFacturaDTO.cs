namespace Entities.DTOs.Facturacion
{
    public class ItemFacturaDTO
    {
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal alicuota_iva { get; set; } = 21; // Solo 21% por ahora
    }
}
