using Entities;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Business.Interfaces
{
    public interface IClienteBusiness : IRepository<Cliente>
    {
        Task<ClienteFacturacionDTO> ValidarClienteAFIP(ValidarClienteRequestDTO request);
    }
}
