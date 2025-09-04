using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClienteBusiness : IClienteBusiness
    {
        private readonly IRepositorio<Cliente> _clienteRepo;

        public ClienteBusiness(IRepositorio<Cliente> clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<List<Cliente>> GetAll()
        {
            var items = await _clienteRepo.GetAll();
            return items.Where(c => c.Activo).ToList();
        }

        public async Task<Cliente?> Get(string id)
        {
            var cliente = await _clienteRepo.Get(id);
            if (cliente == null || !cliente.Activo) return null;
            return cliente;
        }

        public async Task<Cliente> Add(Cliente cliente)
        {
            cliente.Activo = true;
            cliente.FechaCreacion = DateTime.UtcNow;
            return await _clienteRepo.Add(cliente);
        }

        public async Task Update(string id, Cliente cliente)
        {
            cliente.FechaLog = DateTime.UtcNow;
            await _clienteRepo.Update(id, cliente);
        }

        public async Task Delete(string id)
        {
            var cliente = await _clienteRepo.Get(id);
            if (cliente != null)
            {
                cliente.Activo = false;
                cliente.FechaLog = DateTime.UtcNow;
                await _clienteRepo.Update(id, cliente);
            }
        }
    }
}
