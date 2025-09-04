using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class MovimientoStockBusiness : IMovimientoStockBusiness
    {
        private readonly IRepository<MovimientoStock> _movimientoStockRepo;
        private readonly IProductoBusiness _productoBusiness;

        public MovimientoStockBusiness(IRepository<MovimientoStock> movimientoStockRepo, IProductoBusiness productoBusiness)
        {
            _movimientoStockRepo = movimientoStockRepo;
            _productoBusiness = productoBusiness;
        }

        public async Task<string> RegistrarMovimiento(MovimientoStock movimiento)
        {
            // TODO: This operation should be transactional to ensure data integrity.
            if (movimiento == null) throw new ArgumentNullException(nameof(movimiento));
            if (string.IsNullOrEmpty(movimiento.IdProducto)) throw new ArgumentException("El IdProducto es obligatorio.");

            var producto = await _productoBusiness.Get(movimiento.IdProducto);

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
            await _productoBusiness.Update(producto);

            // Create movement record
            return await _movimientoStockRepo.Add(movimiento);
        }

        public async Task<IEnumerable<MovimientoStock>> ObtenerMovimientosPorProducto(string idProducto)
        {
            var movimientos = await _movimientoStockRepo.GetAll();
            return movimientos.Where(m => m.IdProducto == idProducto);
        }
    }
}
