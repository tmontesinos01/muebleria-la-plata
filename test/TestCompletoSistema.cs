using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Entities.DTOs.Facturacion;
using Entities.DTOs.Notas;
using Entities.DTOs.Clientes;

namespace TestProject
{
    /// <summary>
    /// Test completo del sistema de facturación electrónica AFIP/ARCA
    /// Este test simula todo el proceso desde la creación de productos hasta la emisión de comprobantes
    /// </summary>
    public class TestCompletoSistema
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly List<string> _productosCreados = new List<string>();
        private readonly List<string> _ventasCreadas = new List<string>();
        private readonly List<string> _facturasEmitidas = new List<string>();
        private readonly List<string> _notasEmitidas = new List<string>();
        private readonly List<string> _ticketsEmitidos = new List<string>();

        public TestCompletoSistema(string baseUrl = "https://localhost:7000")
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Ejecuta el test completo del sistema
        /// </summary>
        public async Task<TestResult> EjecutarTestCompleto()
        {
            var resultado = new TestResult();
            Console.WriteLine("🚀 INICIANDO TEST COMPLETO DEL SISTEMA DE FACTURACIÓN");
            Console.WriteLine("=" * 60);

            try
            {
                // 1. Configurar datos de prueba
                await ConfigurarDatosDePrueba();

                // 2. Obtener productos (usar existentes o crear nuevos)
                await ObtenerProductos(resultado);

                // 3. Obtener clientes (usar existentes o crear nuevos)
                await ObtenerClientes(resultado);

                // 4. Validar clientes en AFIP
                await ValidarClientesAFIP(resultado);

                // 5. Simular ventas para facturación
                await SimularVentas(resultado);

                // 6. Emitir facturas
                await EmitirFacturas(resultado);

                // 7. Emitir tickets
                await EmitirTickets(resultado);

                // 8. Emitir notas de crédito
                await EmitirNotasCredito(resultado);

                // 9. Emitir notas de débito
                await EmitirNotasDebito(resultado);

                // 10. Anular facturas
                await AnularFacturas(resultado);

                // 11. Reimprimir facturas
                await ReimprimirFacturas(resultado);

                // 12. Consultar datos
                await ConsultarDatos(resultado);

                resultado.Exito = true;
                resultado.Mensaje = "Test completo ejecutado exitosamente";

                Console.WriteLine("\n✅ TEST COMPLETO FINALIZADO EXITOSAMENTE");
                Console.WriteLine("=" * 60);
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error en el test: {ex.Message}";
                resultado.Errores.Add(ex.Message);
                Console.WriteLine($"\n❌ ERROR EN EL TEST: {ex.Message}");
            }

            return resultado;
        }

        /// <summary>
        /// Configura los datos de prueba necesarios
        /// </summary>
        private async Task ConfigurarDatosDePrueba()
        {
            Console.WriteLine("\n📋 CONFIGURANDO DATOS DE PRUEBA...");

            // Verificar que el servidor esté funcionando
            var response = await _httpClient.GetAsync($"{_baseUrl}/swagger");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Servidor no disponible en {_baseUrl}");
            }

            Console.WriteLine("✅ Servidor disponible");
        }

        /// <summary>
        /// Obtiene productos existentes o crea nuevos si es necesario
        /// </summary>
        private async Task ObtenerProductos(TestResult resultado)
        {
            Console.WriteLine("\n📦 OBTENIENDO PRODUCTOS...");

            // Primero consultar productos existentes
            var response = await _httpClient.GetAsync($"{_baseUrl}/productos/obtener-todos");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var productosExistentes = JsonSerializer.Deserialize<JsonElement[]>(responseContent);
                
                Console.WriteLine($"📋 Productos existentes encontrados: {productosExistentes.Length}");

                // Agregar productos existentes a la lista
                foreach (var producto in productosExistentes)
                {
                    if (producto.TryGetProperty("id", out var idElement))
                    {
                        _productosCreados.Add(idElement.GetString());
                        var nombre = producto.TryGetProperty("nombre", out var nombreElement) ? nombreElement.GetString() : "Sin nombre";
                        Console.WriteLine($"✅ Producto existente: {nombre}");
                    }
                }

                // Si hay menos de 5 productos, crear los faltantes
                if (productosExistentes.Length < 5)
                {
                    var productosFaltantes = 5 - productosExistentes.Length;
                    Console.WriteLine($"📝 Creando {productosFaltantes} productos adicionales...");
                    
                    await CrearProductosFaltantes(productosFaltantes, resultado);
                }
                else
                {
                    Console.WriteLine("✅ Suficientes productos existentes para el test");
                }
            }
            else
            {
                Console.WriteLine("❌ Error consultando productos existentes, creando productos nuevos...");
                await CrearProductosFaltantes(5, resultado);
            }
        }

        /// <summary>
        /// Crea productos faltantes
        /// </summary>
        private async Task CrearProductosFaltantes(int cantidad, TestResult resultado)
        {
            var productos = new[]
            {
                new { Nombre = "Mesa de Comedor", Precio = 50000.00m, Stock = 10, Descripcion = "Mesa de comedor de madera maciza", Codigo = "MESA001", AlicuotaIva = 21 },
                new { Nombre = "Silla de Comedor", Precio = 15000.00m, Stock = 50, Descripcion = "Silla de comedor con tapizado", Codigo = "SILLA001", AlicuotaIva = 21 },
                new { Nombre = "Sofá 3 Cuerpos", Precio = 120000.00m, Stock = 5, Descripcion = "Sofá de 3 cuerpos con tapizado premium", Codigo = "SOFA001", AlicuotaIva = 21 },
                new { Nombre = "Mesa de Centro", Precio = 25000.00m, Stock = 15, Descripcion = "Mesa de centro de vidrio templado", Codigo = "MESA002", AlicuotaIva = 21 },
                new { Nombre = "Lámpara de Pie", Precio = 18000.00m, Stock = 20, Descripcion = "Lámpara de pie con pantalla de tela", Codigo = "LAMP001", AlicuotaIva = 21 }
            };

            for (int i = 0; i < Math.Min(cantidad, productos.Length); i++)
            {
                var producto = productos[i];
                var productoData = new
                {
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    Descripcion = producto.Descripcion,
                    Codigo = producto.Codigo,
                    AlicuotaIva = producto.AlicuotaIva,
                    IdCategoria = "",
                    IdUnidadMedida = "",
                    ImagenUrl = "",
                    Activo = true
                };

                var json = JsonSerializer.Serialize(productoData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/productos/crear", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var productoResponse = JsonSerializer.Deserialize<dynamic>(responseContent);
                    _productosCreados.Add(productoResponse.GetProperty("id").GetString());
                    Console.WriteLine($"✅ Producto creado: {producto.Nombre}");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error creando producto {producto.Nombre}: {error}");
                    resultado.Errores.Add($"Error creando producto {producto.Nombre}: {error}");
                }
            }
        }

        /// <summary>
        /// Obtiene clientes existentes o crea nuevos si es necesario
        /// </summary>
        private async Task ObtenerClientes(TestResult resultado)
        {
            Console.WriteLine("\n👥 OBTENIENDO CLIENTES...");

            // Primero consultar clientes existentes
            var response = await _httpClient.GetAsync($"{_baseUrl}/clientes/obtener-todos");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var clientesExistentes = JsonSerializer.Deserialize<JsonElement[]>(responseContent);
                
                Console.WriteLine($"📋 Clientes existentes encontrados: {clientesExistentes.Length}");

                // Si hay menos de 3 clientes, crear los faltantes
                if (clientesExistentes.Length < 3)
                {
                    var clientesFaltantes = 3 - clientesExistentes.Length;
                    Console.WriteLine($"📝 Creando {clientesFaltantes} clientes adicionales...");
                    
                    await CrearClientesFaltantes(clientesFaltantes, resultado);
                }
                else
                {
                    Console.WriteLine("✅ Suficientes clientes existentes para el test");
                }
            }
            else
            {
                Console.WriteLine("❌ Error consultando clientes existentes, creando clientes nuevos...");
                await CrearClientesFaltantes(3, resultado);
            }
        }

        /// <summary>
        /// Crea clientes faltantes
        /// </summary>
        private async Task CrearClientesFaltantes(int cantidad, TestResult resultado)
        {
            var clientes = new[]
            {
                new { 
                    Nombre = "Juan", 
                    Apellido = "Pérez",
                    NumeroDocumento = "12345678", 
                    TipoDocumento = "DNI",
                    Email = "juan.perez@email.com",
                    Telefono = "011-1234-5678",
                    Direccion = "Av. Corrientes 1234"
                },
                new { 
                    Nombre = "María", 
                    Apellido = "González",
                    NumeroDocumento = "87654321", 
                    TipoDocumento = "DNI",
                    Email = "maria.gonzalez@email.com",
                    Telefono = "011-8765-4321",
                    Direccion = "Av. Rivadavia 9876"
                },
                new { 
                    Nombre = "Empresa ABC S.A.", 
                    Apellido = "",
                    NumeroDocumento = "20123456789", 
                    TipoDocumento = "CUIT",
                    Email = "contacto@empresaabc.com",
                    Telefono = "011-9876-5432",
                    Direccion = "Av. Santa Fe 5678"
                }
            };

            for (int i = 0; i < Math.Min(cantidad, clientes.Length); i++)
            {
                var cliente = clientes[i];
                var clienteData = new
                {
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    NumeroDocumento = cliente.NumeroDocumento,
                    TipoDocumento = cliente.TipoDocumento,
                    Email = cliente.Email,
                    Telefono = cliente.Telefono,
                    Direccion = cliente.Direccion,
                    Activo = true
                };

                var json = JsonSerializer.Serialize(clienteData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/clientes/crear", content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Cliente creado: {cliente.Nombre} {cliente.Apellido}");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error creando cliente {cliente.Nombre}: {error}");
                    resultado.Errores.Add($"Error creando cliente {cliente.Nombre}: {error}");
                }
            }
        }

        /// <summary>
        /// Valida clientes en AFIP
        /// </summary>
        private async Task ValidarClientesAFIP(TestResult resultado)
        {
            Console.WriteLine("\n🔍 VALIDANDO CLIENTES EN AFIP...");

            var validaciones = new[]
            {
                new { Documento = "12345678", TipoDocumento = "DNI" },
                new { Documento = "20123456789", TipoDocumento = "CUIT" }
            };

            foreach (var validacion in validaciones)
            {
                var validacionData = new
                {
                    documento_nro = validacion.Documento,
                    documento_tipo = validacion.TipoDocumento
                };

                var json = JsonSerializer.Serialize(validacionData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/clientes/validar-cuit", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var validacionResponse = JsonSerializer.Deserialize<ClienteFacturacionDTO>(responseContent);
                    if (validacionResponse.es_valido)
                    {
                        Console.WriteLine($"✅ Cliente validado en AFIP: {validacion.Documento}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Cliente no validado en AFIP: {validacion.Documento} - {string.Join(", ", validacionResponse.errores)}");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error validando cliente {validacion.Documento}: {error}");
                    resultado.Errores.Add($"Error validando cliente {validacion.Documento}: {error}");
                }
            }
        }

        /// <summary>
        /// Simula ventas para facturación (no se crean en BD, solo se usan para facturar)
        /// </summary>
        private async Task SimularVentas(TestResult resultado)
        {
            Console.WriteLine("\n🛒 SIMULANDO VENTAS PARA FACTURACIÓN...");

            // Simulamos que tenemos ventas con estos datos para facturar
            _ventasCreadas.Add("venta_simulada_1");
            _ventasCreadas.Add("venta_simulada_2");

            Console.WriteLine("✅ Ventas simuladas para facturación");
        }

        /// <summary>
        /// Emite facturas
        /// </summary>
        private async Task EmitirFacturas(TestResult resultado)
        {
            Console.WriteLine("\n🧾 EMITIENDO FACTURAS...");

            var facturas = new[]
            {
                new
                {
                    TipoComprobante = "factura_b",
                    Cliente = new
                    {
                        documento_tipo = "DNI",
                        documento_nro = "12345678",
                        razon_social = "Juan Pérez",
                        direccion = "Av. Corrientes 1234",
                        localidad = "CABA",
                        provincia = "Buenos Aires",
                        codigopostal = "1043",
                        condicion_iva = "5"
                    },
                    Items = new[]
                    {
                        new
                        {
                            descripcion = "Mesa de Comedor",
                            cantidad = 1,
                            precio_unitario = 50000.00m,
                            alicuota_iva = 21
                        },
                        new
                        {
                            descripcion = "Silla de Comedor",
                            cantidad = 4,
                            precio_unitario = 15000.00m,
                            alicuota_iva = 21
                        }
                    }
                },
                new
                {
                    TipoComprobante = "factura_a",
                    Cliente = new
                    {
                        documento_tipo = "CUIT",
                        documento_nro = "20123456789",
                        razon_social = "Empresa ABC S.A.",
                        direccion = "Av. Santa Fe 5678",
                        localidad = "CABA",
                        provincia = "Buenos Aires",
                        codigopostal = "1060",
                        condicion_iva = "1"
                    },
                    Items = new[]
                    {
                        new
                        {
                            descripcion = "Sofá 3 Cuerpos",
                            cantidad = 1,
                            precio_unitario = 120000.00m,
                            alicuota_iva = 21
                        }
                    }
                }
            };

            foreach (var factura in facturas)
            {
                var facturaData = new
                {
                    cliente = factura.Cliente,
                    tipo_comprobante = factura.TipoComprobante,
                    items = factura.Items,
                    observaciones = "Factura de prueba del sistema"
                };

                var json = JsonSerializer.Serialize(facturaData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/facturacion/emitir-factura", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var facturaResponse = JsonSerializer.Deserialize<EmitirFacturaResponseDTO>(responseContent);
                    if (facturaResponse.exito)
                    {
                        _facturasEmitidas.Add(facturaResponse.factura.numero_factura);
                        Console.WriteLine($"✅ Factura emitida: {facturaResponse.factura.numero_factura} - CAE: {facturaResponse.factura.cae}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Error emitiendo factura: {string.Join(", ", facturaResponse.errores)}");
                        resultado.Errores.Add($"Error emitiendo factura: {string.Join(", ", facturaResponse.errores)}");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error emitiendo factura: {error}");
                    resultado.Errores.Add($"Error emitiendo factura: {error}");
                }
            }
        }

        /// <summary>
        /// Emite tickets
        /// </summary>
        private async Task EmitirTickets(TestResult resultado)
        {
            Console.WriteLine("\n🎫 EMITIENDO TICKETS...");

            var tickets = new[]
            {
                new
                {
                    TipoComprobante = "ticket",
                    Cliente = new
                    {
                        documento_tipo = "DNI",
                        documento_nro = "87654321",
                        razon_social = "Consumidor Final",
                        direccion = "Av. Rivadavia 9876",
                        localidad = "CABA",
                        provincia = "Buenos Aires",
                        codigopostal = "1033",
                        condicion_iva = "5"
                    },
                    Items = new[]
                    {
                        new
                        {
                            descripcion = "Mesa de Centro",
                            cantidad = 1,
                            precio_unitario = 25000.00m,
                            alicuota_iva = 21
                        }
                    }
                },
                new
                {
                    TipoComprobante = "ticket_factura_b",
                    Cliente = new
                    {
                        documento_tipo = "DNI",
                        documento_nro = "11223344",
                        razon_social = "Consumidor Final",
                        direccion = "Av. Callao 1111",
                        localidad = "CABA",
                        provincia = "Buenos Aires",
                        codigopostal = "1023",
                        condicion_iva = "5"
                    },
                    Items = new[]
                    {
                        new
                        {
                            descripcion = "Lámpara de Pie",
                            cantidad = 2,
                            precio_unitario = 18000.00m,
                            alicuota_iva = 21
                        }
                    }
                }
            };

            foreach (var ticket in tickets)
            {
                var ticketData = new
                {
                    cliente = ticket.Cliente,
                    tipo_comprobante = ticket.TipoComprobante,
                    items = ticket.Items,
                    observaciones = "Ticket de prueba del sistema"
                };

                var json = JsonSerializer.Serialize(ticketData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/facturacion/emitir-factura", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var ticketResponse = JsonSerializer.Deserialize<EmitirFacturaResponseDTO>(responseContent);
                    if (ticketResponse.exito)
                    {
                        _ticketsEmitidos.Add(ticketResponse.factura.numero_factura);
                        Console.WriteLine($"✅ Ticket emitido: {ticketResponse.factura.numero_factura} - CAE: {ticketResponse.factura.cae}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Error emitiendo ticket: {string.Join(", ", ticketResponse.errores)}");
                        resultado.Errores.Add($"Error emitiendo ticket: {string.Join(", ", ticketResponse.errores)}");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error emitiendo ticket: {error}");
                    resultado.Errores.Add($"Error emitiendo ticket: {error}");
                }
            }
        }

        /// <summary>
        /// Emite notas de crédito
        /// </summary>
        private async Task EmitirNotasCredito(TestResult resultado)
        {
            Console.WriteLine("\n📝 EMITIENDO NOTAS DE CRÉDITO...");

            if (_facturasEmitidas.Count > 0)
            {
                var notaCreditoData = new
                {
                    tipo_nota = "nota_credito",
                    id_factura_original = _facturasEmitidas[0],
                    motivo = "Devolución de producto por defecto",
                    items = new[]
                    {
                        new
                        {
                            id_producto = _productosCreados[1],
                            descripcion = "Silla de Comedor",
                            cantidad = 1,
                            precio_unitario = 15000.00m,
                            alicuota_iva = 21,
                            subtotal = 15000.00m,
                            iva = 3150.00m,
                            total = 18150.00m
                        }
                    },
                    observaciones = "Nota de crédito de prueba del sistema",
                    usuario_emision = "SistemaTest"
                };

                var json = JsonSerializer.Serialize(notaCreditoData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/ventas/emitir-nota-credito", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var notaResponse = JsonSerializer.Deserialize<EmitirNotaResponseDTO>(responseContent);
                    if (notaResponse.exito)
                    {
                        _notasEmitidas.Add(notaResponse.nota.numero_nota);
                        Console.WriteLine($"✅ Nota de crédito emitida: {notaResponse.nota.numero_nota} - CAE: {notaResponse.nota.cae}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Error emitiendo nota de crédito: {string.Join(", ", notaResponse.errores)}");
                        resultado.Errores.Add($"Error emitiendo nota de crédito: {string.Join(", ", notaResponse.errores)}");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error emitiendo nota de crédito: {error}");
                    resultado.Errores.Add($"Error emitiendo nota de crédito: {error}");
                }
            }
        }

        /// <summary>
        /// Emite notas de débito
        /// </summary>
        private async Task EmitirNotasDebito(TestResult resultado)
        {
            Console.WriteLine("\n📝 EMITIENDO NOTAS DE DÉBITO...");

            if (_facturasEmitidas.Count > 1)
            {
                var notaDebitoData = new
                {
                    tipo_nota = "nota_debito",
                    id_factura_original = _facturasEmitidas[1],
                    motivo = "Cargo adicional por servicio de entrega",
                    items = new[]
                    {
                        new
                        {
                            id_producto = "servicio_entrega",
                            descripcion = "Servicio de Entrega a Domicilio",
                            cantidad = 1,
                            precio_unitario = 5000.00m,
                            alicuota_iva = 21,
                            subtotal = 5000.00m,
                            iva = 1050.00m,
                            total = 6050.00m
                        }
                    },
                    observaciones = "Nota de débito de prueba del sistema",
                    usuario_emision = "SistemaTest"
                };

                var json = JsonSerializer.Serialize(notaDebitoData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/ventas/emitir-nota-debito", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var notaResponse = JsonSerializer.Deserialize<EmitirNotaResponseDTO>(responseContent);
                    if (notaResponse.exito)
                    {
                        _notasEmitidas.Add(notaResponse.nota.numero_nota);
                        Console.WriteLine($"✅ Nota de débito emitida: {notaResponse.nota.numero_nota} - CAE: {notaResponse.nota.cae}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Error emitiendo nota de débito: {string.Join(", ", notaResponse.errores)}");
                        resultado.Errores.Add($"Error emitiendo nota de débito: {string.Join(", ", notaResponse.errores)}");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error emitiendo nota de débito: {error}");
                    resultado.Errores.Add($"Error emitiendo nota de débito: {error}");
                }
            }
        }

        /// <summary>
        /// Anula facturas
        /// </summary>
        private async Task AnularFacturas(TestResult resultado)
        {
            Console.WriteLine("\n❌ ANULANDO FACTURAS...");

            if (_facturasEmitidas.Count > 0)
            {
                var anulacionData = new
                {
                    motivo_anulacion = "Anulación de prueba del sistema",
                    observaciones_anulacion = "Factura anulada para testing",
                    usuario_anulacion = "SistemaTest"
                };

                var json = JsonSerializer.Serialize(anulacionData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/facturacion/anular-factura/{_facturasEmitidas[0]}", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var anulacionResponse = JsonSerializer.Deserialize<AnularFacturaResponseDTO>(responseContent);
                    if (anulacionResponse.exito)
                    {
                        Console.WriteLine($"✅ Factura anulada: {anulacionResponse.factura.numero_factura}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Error anulando factura: {string.Join(", ", anulacionResponse.errores)}");
                        resultado.Errores.Add($"Error anulando factura: {string.Join(", ", anulacionResponse.errores)}");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error anulando factura: {error}");
                    resultado.Errores.Add($"Error anulando factura: {error}");
                }
            }
        }

        /// <summary>
        /// Reimprime facturas
        /// </summary>
        private async Task ReimprimirFacturas(TestResult resultado)
        {
            Console.WriteLine("\n🖨️ REIMPRIMIENDO FACTURAS...");

            if (_facturasEmitidas.Count > 1)
            {
                var reimpresionData = new
                {
                    motivo_reimpresion = "Reimpresión de prueba del sistema",
                    usuario_reimpresion = "SistemaTest"
                };

                var json = JsonSerializer.Serialize(reimpresionData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/facturacion/reimprimir-factura/{_facturasEmitidas[1]}", content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Factura reimpresa: {_facturasEmitidas[1]}");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error reimprimiendo factura: {error}");
                    resultado.Errores.Add($"Error reimprimiendo factura: {error}");
                }
            }
        }

        /// <summary>
        /// Consulta todos los datos creados
        /// </summary>
        private async Task ConsultarDatos(TestResult resultado)
        {
            Console.WriteLine("\n📊 CONSULTANDO DATOS CREADOS...");

            // Consultar productos
            var productosResponse = await _httpClient.GetAsync($"{_baseUrl}/productos/obtener-todos");
            if (productosResponse.IsSuccessStatusCode)
            {
                var productosContent = await productosResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"✅ Productos consultados: {productosContent.Length} caracteres");
            }
            else
            {
                Console.WriteLine($"❌ Error consultando productos: {productosResponse.StatusCode}");
            }

            // Consultar clientes
            var clientesResponse = await _httpClient.GetAsync($"{_baseUrl}/clientes/obtener-todos");
            if (clientesResponse.IsSuccessStatusCode)
            {
                var clientesContent = await clientesResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"✅ Clientes consultados: {clientesContent.Length} caracteres");
            }
            else
            {
                Console.WriteLine($"❌ Error consultando clientes: {clientesResponse.StatusCode}");
            }

            // Consultar ventas pendientes de facturación
            var ventasResponse = await _httpClient.GetAsync($"{_baseUrl}/ventas/pendientes-facturacion");
            if (ventasResponse.IsSuccessStatusCode)
            {
                var ventasContent = await ventasResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"✅ Ventas pendientes consultadas: {ventasContent.Length} caracteres");
            }
            else
            {
                Console.WriteLine($"❌ Error consultando ventas: {ventasResponse.StatusCode}");
            }

            // Consultar notas por factura si hay facturas emitidas
            if (_facturasEmitidas.Count > 0)
            {
                var notasResponse = await _httpClient.GetAsync($"{_baseUrl}/ventas/consultar-notas-por-factura/{_facturasEmitidas[0]}");
                if (notasResponse.IsSuccessStatusCode)
                {
                    var notasContent = await notasResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"✅ Notas consultadas: {notasContent.Length} caracteres");
                }
                else
                {
                    Console.WriteLine($"❌ Error consultando notas: {notasResponse.StatusCode}");
                }
            }
        }

        /// <summary>
        /// Obtiene el resumen de todos los datos creados
        /// </summary>
        public TestSummary ObtenerResumen()
        {
            return new TestSummary
            {
                ProductosCreados = _productosCreados.Count,
                VentasCreadas = _ventasCreadas.Count,
                FacturasEmitidas = _facturasEmitidas.Count,
                TicketsEmitidos = _ticketsEmitidos.Count,
                NotasEmitidas = _notasEmitidas.Count,
                ProductosIds = _productosCreados,
                VentasIds = _ventasCreadas,
                FacturasNumeros = _facturasEmitidas,
                TicketsNumeros = _ticketsEmitidos,
                NotasNumeros = _notasEmitidas
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    /// <summary>
    /// Resultado del test completo
    /// </summary>
    public class TestResult
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public List<string> Errores { get; set; } = new List<string>();
        public DateTime FechaEjecucion { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Resumen de datos creados en el test
    /// </summary>
    public class TestSummary
    {
        public int ProductosCreados { get; set; }
        public int VentasCreadas { get; set; }
        public int FacturasEmitidas { get; set; }
        public int TicketsEmitidos { get; set; }
        public int NotasEmitidas { get; set; }
        public List<string> ProductosIds { get; set; } = new List<string>();
        public List<string> VentasIds { get; set; } = new List<string>();
        public List<string> FacturasNumeros { get; set; } = new List<string>();
        public List<string> TicketsNumeros { get; set; } = new List<string>();
        public List<string> NotasNumeros { get; set; } = new List<string>();
    }
}
