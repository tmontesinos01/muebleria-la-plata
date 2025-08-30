namespace Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
    }
}