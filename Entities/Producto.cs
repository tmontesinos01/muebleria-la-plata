namespace Entities
{
    public class Producto : EntidadMaestra
    {
        public int IdCategoria { get; set; } 
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public string? Descripcion { get; set; }
        public virtual Categoria? Categoria { get; set; }
    }
}