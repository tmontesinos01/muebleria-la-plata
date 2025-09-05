using Business.Interfaces;
using Data.Interfaces;
using Entities;
using Entities.DTOs.Notas;
using Entities.DTOs.Facturacion;
using Entities.DTOs.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class VentaBusiness : IVentaBusiness
    {
        private readonly IRepository<Venta> _ventaRepo;
        private readonly IRepository<VentaDetalle> _ventaDetalleRepo;
        private readonly IMovimientoStockBusiness _movimientoStockBusiness;
        private readonly IFacturaBusiness _facturaBusiness;
        private readonly IFacturacionBusiness _facturacionBusiness;

        public VentaBusiness(
            IRepository<Venta> ventaRepo, 
            IRepository<VentaDetalle> ventaDetalleRepo, 
            IMovimientoStockBusiness movimientoStockBusiness,
            IFacturaBusiness facturaBusiness,
            IFacturacionBusiness facturacionBusiness)
        {
            _ventaRepo = ventaRepo;
            _ventaDetalleRepo = ventaDetalleRepo;
            _movimientoStockBusiness = movimientoStockBusiness;
            _facturaBusiness = facturaBusiness;
            _facturacionBusiness = facturacionBusiness;
        }

        public async Task<string> Add(Venta venta)
        {
            if (venta == null) throw new ArgumentNullException(nameof(venta));

            // TODO: This operation should be transactional.
            venta.Activo = true;
            venta.FechaCreacion = DateTime.UtcNow;

            // 1. Create the main Venta and get its ID
            var ventaId = await _ventaRepo.Add(venta);
            venta.Id = ventaId;

            // 2. Save Details and update Stock
            if (venta.Detalles != null)
            {
                foreach (var detalle in venta.Detalles)
                {
                    detalle.IdVenta = ventaId;
                    detalle.Activo = true;
                    detalle.FechaCreacion = DateTime.UtcNow;
                    
                    // Calcular automáticamente IVA y Total
                    detalle.CalcularIVA();
                    
                    await _ventaDetalleRepo.Add(detalle);

                    var movimiento = new MovimientoStock
                    {
                        IdProducto = detalle.IdProducto,
                        Fecha = venta.FechaVenta,
                        Cantidad = -detalle.Cantidad, // Sale decreases stock
                        TipoMovimiento = "Venta",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow
                    };
                    await _movimientoStockBusiness.RegistrarMovimiento(movimiento);
                }
            }
            return ventaId;
        }

        public int CrearVenta(Venta datos)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            var venta = await _ventaRepo.Get(id);
            if (venta != null)
            {
                venta.Activo = false;
                venta.FechaLog = DateTime.UtcNow;
                await _ventaRepo.Update(venta);
            }
        }

        public async Task<Venta?> Get(string id)
        {
            var venta = await _ventaRepo.Get(id);
            if (venta == null || !venta.Activo) return null;
            return venta;
        }

        public async Task<IEnumerable<Venta>> GetAll()
        {
            var ventas = await _ventaRepo.GetAll();
            return ventas.Where(v => v.Activo);
        }

        public Venta ObtenerVentaPorId(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Venta venta)
        {
            venta.FechaLog = DateTime.UtcNow;
            await _ventaRepo.Update(venta);
        }

        public async Task<IEnumerable<Venta>> GetVentasPendientesFacturacion()
        {
            var ventas = await GetAll();
            return ventas.Where(v => !v.Facturada && v.EstadoFacturacion == "PENDIENTE");
        }

        public async Task<bool> MarcarVentaComoFacturada(string ventaId, string numeroFactura)
        {
            var venta = await Get(ventaId);
            if (venta == null) return false;

            venta.Facturada = true;
            venta.NumeroFactura = numeroFactura;
            venta.FechaFacturacion = DateTime.UtcNow;
            venta.EstadoFacturacion = "FACTURADA";
            venta.FechaLog = DateTime.UtcNow;

            await Update(venta);
            return true;
        }

        public async Task<EmitirNotaResponseDTO> EmitirNotaCredito(EmitirNotaRequestDTO request)
        {
            return await EmitirNota(request, "nota_credito");
        }

        public async Task<EmitirNotaResponseDTO> EmitirNotaDebito(EmitirNotaRequestDTO request)
        {
            return await EmitirNota(request, "nota_debito");
        }

        private async Task<EmitirNotaResponseDTO> EmitirNota(EmitirNotaRequestDTO request, string tipoNota)
        {
            try
            {
                // Validar parámetros de entrada
                if (request == null || string.IsNullOrEmpty(request.id_factura_original))
                {
                    return new EmitirNotaResponseDTO
                    {
                        exito = false,
                        mensaje = "Datos de nota inválidos",
                        errores = new List<string> { "ID de factura original es requerido" }
                    };
                }

                // Obtener la factura original
                var facturaOriginal = await _facturaBusiness.Get(request.id_factura_original);
                if (facturaOriginal == null)
                {
                    return new EmitirNotaResponseDTO
                    {
                        exito = false,
                        mensaje = "Factura original no encontrada",
                        errores = new List<string> { $"Factura con ID '{request.id_factura_original}' no existe" }
                    };
                }

                // Validar que la factura original esté en estado válido
                if (facturaOriginal.Estado == "ANULADA")
                {
                    return new EmitirNotaResponseDTO
                    {
                        exito = false,
                        mensaje = "No se puede emitir nota sobre una factura anulada",
                        errores = new List<string> { "La factura original está anulada" }
                    };
                }

                // Preparar la solicitud para la emisión de la nota
                var emitirFacturaRequest = new EmitirFacturaRequestDTO
                {
                    cliente = new ClienteAFIPDTO
                    {
                        documento_tipo = facturaOriginal.ClienteDocumentoTipo,
                        documento_nro = facturaOriginal.ClienteDocumentoNro,
                        razon_social = facturaOriginal.ClienteRazonSocial,
                        direccion = facturaOriginal.ClienteDireccion,
                        localidad = facturaOriginal.ClienteLocalidad,
                        provincia = facturaOriginal.ClienteProvincia,
                        codigopostal = facturaOriginal.ClienteCodigoPostal,
                        condicion_iva = facturaOriginal.ClienteCondicionIVA
                    },
                    tipo_comprobante = tipoNota,
                    items = request.items?.Select(item => new ItemFacturaDTO
                    {
                        descripcion = item.descripcion,
                        cantidad = item.cantidad,
                        precio_unitario = item.precio_unitario,
                        alicuota_iva = item.alicuota_iva
                    }).ToList() ?? new List<ItemFacturaDTO>(),
                    observaciones = $"{request.observaciones} - Nota emitida sobre factura {facturaOriginal.NumeroFactura}. Motivo: {request.motivo}"
                };

                // Emitir la nota usando el servicio de facturación
                var resultadoEmision = await _facturacionBusiness.EmitirFactura(emitirFacturaRequest);

                if (resultadoEmision.exito)
                {
                    // Crear la entidad Factura para la nota
                    var nota = new Factura
                    {
                        IdVenta = facturaOriginal.IdVenta,
                        NumeroFactura = resultadoEmision.factura.numero_factura,
                        CAE = resultadoEmision.factura.cae,
                        FechaVencimientoCAE = resultadoEmision.factura.fecha_vencimiento_cae,
                        Total = resultadoEmision.factura.total,
                        UrlPDF = resultadoEmision.factura.url_pdf,
                        TipoComprobante = tipoNota,
                        PuntoVenta = resultadoEmision.factura.punto_venta,
                        FechaEmision = DateTime.UtcNow,
                        Estado = "EMITIDA",
                        ClienteDocumentoTipo = facturaOriginal.ClienteDocumentoTipo,
                        ClienteDocumentoNro = facturaOriginal.ClienteDocumentoNro,
                        ClienteRazonSocial = facturaOriginal.ClienteRazonSocial,
                        ClienteDireccion = facturaOriginal.ClienteDireccion,
                        ClienteLocalidad = facturaOriginal.ClienteLocalidad,
                        ClienteProvincia = facturaOriginal.ClienteProvincia,
                        ClienteCodigoPostal = facturaOriginal.ClienteCodigoPostal,
                        ClienteCondicionIVA = facturaOriginal.ClienteCondicionIVA,
                        Observaciones = emitirFacturaRequest.observaciones,
                        IdFacturaOriginal = request.id_factura_original,
                        Items = request.items?.Select(item => new ItemFactura
                        {
                            IdProducto = item.id_producto,
                            Descripcion = item.descripcion,
                            Cantidad = item.cantidad,
                            PrecioUnitario = item.precio_unitario,
                            AlicuotaIVA = item.alicuota_iva,
                            Subtotal = item.subtotal,
                            IVA = item.iva,
                            Total = item.total
                        }).ToList() ?? new List<ItemFactura>(),
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaLog = DateTime.UtcNow,
                        UserLog = request.usuario_emision
                    };

                    // Guardar la nota en la base de datos
                    var notaId = await _facturaBusiness.Add(nota);

                    return new EmitirNotaResponseDTO
                    {
                        exito = true,
                        mensaje = $"{tipoNota.Replace("_", " ").ToUpper()} emitida exitosamente",
                        nota = new NotaEmitidaDTO
                        {
                            id_nota = notaId,
                            numero_nota = nota.NumeroFactura,
                            cae = nota.CAE,
                            fecha_vencimiento_cae = nota.FechaVencimientoCAE,
                            total = nota.Total,
                            url_pdf = nota.UrlPDF,
                            tipo_nota = tipoNota,
                            punto_venta = nota.PuntoVenta,
                            id_factura_original = request.id_factura_original,
                            motivo = request.motivo,
                            fecha_emision = nota.FechaEmision,
                            estado = nota.Estado
                        }
                    };
                }
                else
                {
                    return new EmitirNotaResponseDTO
                    {
                        exito = false,
                        mensaje = "Error en la emisión de la nota",
                        errores = resultadoEmision.errores
                    };
                }
            }
            catch (Exception ex)
            {
                return new EmitirNotaResponseDTO
                {
                    exito = false,
                    mensaje = "Error interno en la emisión de la nota",
                    errores = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ConsultarNotasResponseDTO> ConsultarNotasPorFactura(string facturaId)
        {
            try
            {
                var facturas = await _facturaBusiness.GetAll();
                var notas = facturas.Where(f => f.IdFacturaOriginal == facturaId && f.Activo);

                var notasDTO = notas.Select(nota => new NotaEmitidaDTO
                {
                    id_nota = nota.Id,
                    numero_nota = nota.NumeroFactura,
                    cae = nota.CAE,
                    fecha_vencimiento_cae = nota.FechaVencimientoCAE,
                    total = nota.Total,
                    url_pdf = nota.UrlPDF,
                    tipo_nota = nota.TipoComprobante,
                    punto_venta = nota.PuntoVenta,
                    id_factura_original = nota.IdFacturaOriginal,
                    motivo = nota.Observaciones,
                    fecha_emision = nota.FechaEmision,
                    estado = nota.Estado
                }).ToList();

                return new ConsultarNotasResponseDTO
                {
                    exito = true,
                    mensaje = "Notas consultadas exitosamente",
                    notas = notasDTO
                };
            }
            catch (Exception ex)
            {
                return new ConsultarNotasResponseDTO
                {
                    exito = false,
                    mensaje = "Error interno en la consulta de notas",
                    errores = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ConsultarNotasResponseDTO> ConsultarNotas(ConsultarNotasRequestDTO request)
        {
            try
            {
                var facturas = await _facturaBusiness.GetAll();
                var notas = facturas.Where(f => f.Activo && 
                    (f.TipoComprobante == "nota_credito" || f.TipoComprobante == "nota_debito"));

                // Aplicar filtros
                if (!string.IsNullOrEmpty(request.id_factura_original))
                {
                    notas = notas.Where(n => n.IdFacturaOriginal == request.id_factura_original);
                }

                if (!string.IsNullOrEmpty(request.tipo_nota))
                {
                    notas = notas.Where(n => n.TipoComprobante == request.tipo_nota);
                }

                if (request.fecha_desde.HasValue)
                {
                    notas = notas.Where(n => n.FechaEmision >= request.fecha_desde.Value);
                }

                if (request.fecha_hasta.HasValue)
                {
                    notas = notas.Where(n => n.FechaEmision <= request.fecha_hasta.Value);
                }

                var notasDTO = notas.Select(nota => new NotaEmitidaDTO
                {
                    id_nota = nota.Id,
                    numero_nota = nota.NumeroFactura,
                    cae = nota.CAE,
                    fecha_vencimiento_cae = nota.FechaVencimientoCAE,
                    total = nota.Total,
                    url_pdf = nota.UrlPDF,
                    tipo_nota = nota.TipoComprobante,
                    punto_venta = nota.PuntoVenta,
                    id_factura_original = nota.IdFacturaOriginal,
                    motivo = nota.Observaciones,
                    fecha_emision = nota.FechaEmision,
                    estado = nota.Estado
                }).ToList();

                return new ConsultarNotasResponseDTO
                {
                    exito = true,
                    mensaje = "Notas consultadas exitosamente",
                    notas = notasDTO
                };
            }
            catch (Exception ex)
            {
                return new ConsultarNotasResponseDTO
                {
                    exito = false,
                    mensaje = "Error interno en la consulta de notas",
                    errores = new List<string> { ex.Message }
                };
            }
        }
    }
}
