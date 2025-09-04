using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PerfilesController : ControllerBase
    {
        private readonly IPerfilBusiness _perfilBusiness;

        public PerfilesController(IPerfilBusiness perfilBusiness)
        {
            _perfilBusiness = perfilBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<List<Perfil>>> GetAllPerfiles()
        {
            var datos = await _perfilBusiness.GetAll();
            return datos.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Perfil>> GetPerfilById(string id)
        {
            var perfil = await _perfilBusiness.Get(id);
            if (perfil == null) return NotFound();
            return perfil;
        }

        [HttpPost]
        public async Task<ActionResult<Perfil>> CreatePerfil(Perfil perfil)
        {
            await _perfilBusiness.Add(perfil);
            return CreatedAtAction(nameof(GetPerfilById), new { id = perfil.Id }, perfil);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerfil(string id, Perfil perfil)
        {
            await _perfilBusiness.Update(perfil);
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
