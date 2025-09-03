using Data.Interfaces;
using Entities;

namespace Data.Repositorios
{
    public class ClienteRepositorio : RepositorioBase<Cliente>, IClienteRepositorio
    {
        public ClienteRepositorio() : base("clientes")
        {
        }
    }
}
