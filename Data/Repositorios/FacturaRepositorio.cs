using Data.Interfaces;
using Entities;

namespace Data.Repositorios
{
    public class FacturaRepositorio : RepositorioBase<Factura>, IFacturaRepositorio
    {
        public FacturaRepositorio() : base("facturas")
        {
        }
    }
}
