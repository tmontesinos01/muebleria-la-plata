using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class MovimientoStockBusiness
    {
        private readonly IMovimientoStockRepositorio _movimientoStockRepo;
        private readonly IProductoRepositorio _productoRepo;

        public MovimientoStockBusiness(IMovimientoStockRepositorio movimientoStockRepo, IProductoRepositorio productoRepo)
        {
            _movimientoStockRepo = movimientoStockRepo;
            _productoRepo = productoRepo;
        }

        public async Task<string> RegistrarMovimiento(MovimientoStock movimiento)
        {
            // TODO: This operation should be transactional to ensure data integrity.
            if (movimiento == null) throw new ArgumentNullException(nameof(movimiento));
            if (string.IsNullOrEmpty(movimiento.IdProducto)) throw new ArgumentException("El IdProducto es obligatorio.");

            var producto = await _productoRepo.Get(movimiento.IdProducto);

            if (producto == null)
            {
                throw new KeyNotFoundException($"El producto con ID {movimiento.IdProducto} no fue encontrado.");
            }

            // === STOCK VALIDATION ===
            // If the quantity is negative, it's an outgoing movement (sale). Validate stock.
            if (movimiento.Cantidad < 0)
            {
                var cantidadRequerida = -movimiento.Cantidad; // Positive quantity
                if (producto.Stock < cantidadRequerida)
                {
                    throw new InvalidOperationException($"Stock insuficiente para el producto '{producto.Nombre}'. Stock actual: {producto.Stock}, se requiere: {cantidadRequerida}");
                }
            }

            // Update stock
            producto.Stock += movimiento.Cantidad;
            await _productoRepo.Update(producto.Id, producto);

            // Create movement record
            var createdMovimiento = await _movimientoStockRepo.Add(movimiento);
            return createdMovimiento.Id;
        }

        public async Task<IEnumerable<MovimientoStock>> ObtenerMovimientosPorProducto(string idProducto)
        { 
            var movimientos = await _movimientoStockRepo.GetAll();
            return movimientos.Where(m => m.IdProducto == idProducto);
        }
    }
}
