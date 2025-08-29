using Entities;

namespace Data.Interfaces
{
    public interface IClienteRepositorio
    {
        Cliente GetCliente(int id);
    }
}