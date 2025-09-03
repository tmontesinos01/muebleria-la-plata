using Business.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaBusiness _categoriaBusiness;

        public CategoriasController(CategoriaBusiness categoriaBusiness)
        {
            _categoriaBusiness = categoriaBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetAllCategorias()
        {
            return await _categoriaBusiness.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoriaById(string id)
        {
            var categoria = await _categoriaBusiness.GetById(id);
            if (categoria == null) return NotFound();
            return categoria;
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> CreateCategoria(Categoria categoria)
        {
            await _categoriaBusiness.Create(categoria);
            return CreatedAtAction(nameof(GetCategoriaById), new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(string id, Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest();
            await _categoriaBusiness.Update(id, categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(string id)
        {
            await _categoriaBusiness.Delete(id);
            return NoContent();
        }
    }
}
