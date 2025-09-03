using Data.Interfaces;
using Entities;

namespace Data.Repositorios
{
    public class VentaRepositorio : RepositorioBase<Venta>, IVentaRepositorio
    {
        public VentaRepositorio() : base("ventas")
        {
        }
    }
}
