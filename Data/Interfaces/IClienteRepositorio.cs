using Entities;
using Data.Interfaces;

namespace Data.Interfaces
{
    public interface IClienteRepositorio : IRepositorio<Cliente>
    {
        // We can add specific methods for clients here in the future if needed
    }
}
