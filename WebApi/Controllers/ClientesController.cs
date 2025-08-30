using Business;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IEnumerable<Cliente>> Get()
        {
            return await _clienteService.GetAllClientes();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(string id)
        {
            var cliente = await _clienteService.GetClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Post([FromBody] Cliente cliente)
        {
            var id = await _clienteService.CreateCliente(cliente);
            cliente.Id = id;
            return CreatedAtAction(nameof(Get), new { id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            await _clienteService.UpdateCliente(cliente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _clienteService.DeleteCliente(id);
            return NoContent();
        }
    }
}
