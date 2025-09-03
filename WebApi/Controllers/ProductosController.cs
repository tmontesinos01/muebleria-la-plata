using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoBusiness _productoBusiness;
        private readonly IImagenService _imagenService;

        public ProductosController(IProductoBusiness productoBusiness, IImagenService imagenService)
        {
            _productoBusiness = productoBusiness;
            _imagenService = imagenService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetProductos()
        {
            var productos = await _productoBusiness.GetAll();
            return productos.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(string id)
        {
            var producto = await _productoBusiness.Get(id);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto([FromForm] Producto producto, IFormFile imagen)
        {
            if (imagen != null && imagen.Length > 0)
            {
                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";
                var imageUrl = await _imagenService.SubirImagenAsync(imagen, nombreArchivo);
                producto.ImagenUrl = imageUrl;
            }

            await _productoBusiness.Add(producto);

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(string id, [FromForm] Producto producto, IFormFile? imagen)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            var existingProducto = await _productoBusiness.Get(id);
            if (existingProducto == null)
            {
                return NotFound();
            }

            if (imagen != null && imagen.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingProducto.ImagenUrl))
                {
                    try
                    {
                         await _imagenService.EliminarImagenAsync(existingProducto.ImagenUrl);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting old image: {ex.Message}");
                    }
                }

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";
                var newImageUrl = await _imagenService.SubirImagenAsync(imagen, nombreArchivo);
                producto.ImagenUrl = newImageUrl;
            }
            else
            {
                producto.ImagenUrl = existingProducto.ImagenUrl;
            }


            await _productoBusiness.Update(producto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(string id)
        {
            var producto = await _productoBusiness.Get(id);
            if (producto == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(producto.ImagenUrl))
            {
                 try
                {
                    await _imagenService.EliminarImagenAsync(producto.ImagenUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting image: {ex.Message}");
                }
            }

            await _productoBusiness.Delete(id);
            return NoContent();
        }
    }
}
