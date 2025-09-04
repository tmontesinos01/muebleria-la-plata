using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaBusiness _categoriaBusiness;

        public CategoriasController(ICategoriaBusiness categoriaBusiness)
        {
            _categoriaBusiness = categoriaBusiness;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllCategorias()
        {
            var items = await _categoriaBusiness.GetAll();
            return Ok(items);
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<Categoria>> GetCategoriaById(string id)
        {
            var categoria = await _categoriaBusiness.Get(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Categoria>> CreateCategoria(Categoria categoria)
        {
            var newId = await _categoriaBusiness.Add(categoria);
            categoria.Id = newId;
            return CreatedAtAction(nameof(GetCategoriaById), new { id = newId }, categoria);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateCategoria(string id, Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest();
            await _categoriaBusiness.Update(categoria);
            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteCategoria(string id)
        {
            await _categoriaBusiness.Delete(id);
            return NoContent();
        }
    }
}
