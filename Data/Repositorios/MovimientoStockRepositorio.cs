using Data.Interfaces;
using Entities;

namespace Data.Repositorios
{
    public class MovimientoStockRepositorio : RepositorioBase<MovimientoStock>, IMovimientoStockRepositorio
    {
        public MovimientoStockRepositorio() : base("movimientosStock")
        {
        }
    }
}
