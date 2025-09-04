using Business.Interfaces;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteBusiness _clienteBusiness;

        public ClientesController(IClienteBusiness clienteBusiness)
        {
            _clienteBusiness = clienteBusiness;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllClientes()
        {
            var items = await _clienteBusiness.GetAll();
            return Ok(items);
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<Cliente>> GetClienteById(string id)
        {
            var cliente = await _clienteBusiness.Get(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Cliente>> CreateCliente(Cliente cliente)
        {
            var newId = await _clienteBusiness.Add(cliente);
            cliente.Id = newId;
            return CreatedAtAction(nameof(GetClienteById), new { id = newId }, cliente);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateCliente(string id, Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            await _clienteBusiness.Update(cliente);
            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteCliente(string id)
        {
            await _clienteBusiness.Delete(id);
            return NoContent();
        }

        [HttpPost("validar-cuit")]
        public async Task<ActionResult<ClienteFacturacionDTO>> ValidarClienteAFIP([FromBody] ValidarClienteRequestDTO request)
        {
            var resultado = await _clienteBusiness.ValidarClienteAFIP(request);
            return Ok(resultado);
        }
    }
}
