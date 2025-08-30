
using System;

namespace Entities
{
    public abstract class EntidadMaestra
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaLog { get; set; } = DateTime.UtcNow;
        public string UserLog { get; set; } = "DefaultUser";
        public bool Activo { get; set; } = true;
    }
}
