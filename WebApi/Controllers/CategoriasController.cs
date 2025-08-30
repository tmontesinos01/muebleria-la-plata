using Business;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IEnumerable<Categoria>> Get()
        {
            return await _categoriaService.GetAllCategorias();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(string id)
        {
            var categoria = await _categoriaService.GetCategoriaById(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Post([FromBody] Categoria categoria)
        {
            var id = await _categoriaService.CreateCategoria(categoria);
            categoria.Id = id;
            return CreatedAtAction(nameof(Get), new { id }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            await _categoriaService.UpdateCategoria(categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _categoriaService.DeleteCategoria(id);
            return NoContent();
        }
    }
}
