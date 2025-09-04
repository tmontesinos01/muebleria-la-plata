using Data.Interfaces;
using Entities;

namespace Business.Interfaces
{
    public interface IVentaBusiness : IRepository<Venta>
    {
        public int CrearVenta(Venta datos);
        public Venta ObtenerVentaPorId(string id);
    }
}
