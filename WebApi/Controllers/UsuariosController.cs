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
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioBusiness _usuarioBusiness;

        public UsuariosController(IUsuarioBusiness usuarioBusiness)
        {
            _usuarioBusiness = usuarioBusiness;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAllUsuarios()
        {
            var items = await _usuarioBusiness.GetAll();
            return Ok(items);
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioById(string id)
        {
            var usuario = await _usuarioBusiness.Get(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            var newId = await _usuarioBusiness.Add(usuario);
            usuario.Id = newId;
            return CreatedAtAction(nameof(GetUsuarioById), new { id = newId }, usuario);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateUsuario(string id, Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();
            await _usuarioBusiness.Update(usuario);
            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            await _usuarioBusiness.Delete(id);
            return NoContent();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult<AuthResponseDTO>> Autenticar([FromBody] AuthRequestDTO authRequest)
        {
            var resultado = await _usuarioBusiness.AutenticarUsuario(authRequest);
            return Ok(resultado);
        }
    }
}
