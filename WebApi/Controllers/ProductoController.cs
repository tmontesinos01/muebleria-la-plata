using Business;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoServicio _productoServicio;

        public ProductoController(ProductoServicio productoServicio)
        {
            _productoServicio = productoServicio;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_productoServicio.GetProducto(id));
        }
    }
}