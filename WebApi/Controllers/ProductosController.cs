using Business;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductosController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IEnumerable<Producto>> Get()
        {
            return await _productoService.GetAllProductos();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> Get(string id)
        {
            var producto = await _productoService.GetProductoById(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> Post([FromBody] Producto producto)
        {
            var id = await _productoService.CreateProducto(producto);
            producto.Id = id;
            return CreatedAtAction(nameof(Get), new { id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            await _productoService.UpdateProducto(producto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productoService.DeleteProducto(id);
            return NoContent();
        }
    }
}
