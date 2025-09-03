using Business.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly ConfiguracionBusiness _configuracionBusiness;

        public ConfiguracionesController(ConfiguracionBusiness configuracionBusiness)
        {
            _configuracionBusiness = configuracionBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<List<Configuracion>>> GetAllConfiguraciones()
        {
            return await _configuracionBusiness.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Configuracion>> GetConfiguracionById(string id)
        {
            var configuracion = await _configuracionBusiness.GetById(id);
            if (configuracion == null) return NotFound();
            return configuracion;
        }

        [HttpPost]
        public async Task<ActionResult<Configuracion>> CreateConfiguracion(Configuracion configuracion)
        {
            await _configuracionBusiness.Create(configuracion);
            return CreatedAtAction(nameof(GetConfiguracionById), new { id = configuracion.Id }, configuracion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConfiguracion(string id, Configuracion configuracion)
        {
            if (id != configuracion.Id) return BadRequest();
            await _configuracionBusiness.Update(id, configuracion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfiguracion(string id)
        {
            await _configuracionBusiness.Delete(id);
            return NoContent();
        }
    }
}
