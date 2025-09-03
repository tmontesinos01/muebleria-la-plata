using Business.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfilesController : ControllerBase
    {
        private readonly PerfilBusiness _perfilBusiness;

        public PerfilesController(PerfilBusiness perfilBusiness)
        {
            _perfilBusiness = perfilBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<List<Perfil>>> GetAllPerfiles()
        {
            return await _perfilBusiness.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Perfil>> GetPerfilById(string id)
        {
            var perfil = await _perfilBusiness.GetById(id);
            if (perfil == null) return NotFound();
            return perfil;
        }

        [HttpPost]
        public async Task<ActionResult<Perfil>> CreatePerfil(Perfil perfil)
        {
            await _perfilBusiness.Create(perfil);
            return CreatedAtAction(nameof(GetPerfilById), new { id = perfil.Id }, perfil);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerfil(string id, Perfil perfil)
        {
            if (id != perfil.Id)
            {
                return BadRequest();
            }
            await _perfilBusiness.Update(id, perfil);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerfil(string id)
        {
            await _perfilBusiness.Delete(id);
            return NoContent();
        }
    }
}
