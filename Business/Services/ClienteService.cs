using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClienteService
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteService(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public Task<IEnumerable<Cliente>> GetAllClientes()
        {
            return _clienteRepositorio.GetAll();
        }

        public Task<Cliente> GetClienteById(string id)
        {
            return _clienteRepositorio.Get(id);
        }

        public async Task<string> CreateCliente(Cliente cliente)
        {
            var newCliente = await _clienteRepositorio.Add(cliente);
            return newCliente.Id;
        }

        public Task UpdateCliente(Cliente cliente)
        {
            return _clienteRepositorio.Update(cliente.Id, cliente);
        }

        public Task DeleteCliente(string id)
        {
            return _clienteRepositorio.Delete(id);
        }
    }
}
