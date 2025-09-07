using Entities;
using Entities.DTOs.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Business.Interfaces
{
    public interface IClienteBusiness
    {
        Task<IEnumerable<Cliente>> GetAll();
        Task<Cliente?> Get(string id);
        Task<string> Add(Cliente cliente);
        Task Update(Cliente cliente);
        Task Delete(string id);
        Task<ClienteFacturacionDTO> ValidarClienteAFIP(ValidarClienteRequestDTO request);
    }
}
