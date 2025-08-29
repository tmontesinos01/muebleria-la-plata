using Data.Interfaces;
using Entities;

namespace Business
{
    public class ClienteServicio
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteServicio(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public Cliente GetCliente(int id)
        {
            return _clienteRepositorio.GetCliente(id);
        }
    }
}