using Business;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteServicio _clienteServicio;

        public ClienteController(ClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_clienteServicio.GetCliente(id));
        }
    }
}