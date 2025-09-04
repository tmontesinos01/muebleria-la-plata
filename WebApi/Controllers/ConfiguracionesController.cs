using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly IConfiguracionBusiness _configuracionBusiness;

        public ConfiguracionesController(IConfiguracionBusiness configuracionBusiness)
        {
            _configuracionBusiness = configuracionBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuracion>>> GetAllConfiguraciones()
        {
            var items = await _configuracionBusiness.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Configuracion>> GetConfiguracionById(string id)
        {
            var configuracion = await _configuracionBusiness.Get(id);
            if (configuracion == null) return NotFound();
            return Ok(configuracion);
        }

        [HttpPost]
        public async Task<ActionResult<Configuracion>> CreateConfiguracion(Configuracion configuracion)
        {
            var newId = await _configuracionBusiness.Add(configuracion);
            configuracion.Id = newId;
            return CreatedAtAction(nameof(GetConfiguracionById), new { id = newId }, configuracion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConfiguracion(string id, Configuracion configuracion)
        {
            if (id != configuracion.Id) return BadRequest();
            await _configuracionBusiness.Update(configuracion);
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
