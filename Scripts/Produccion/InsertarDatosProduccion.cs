using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Apis.Auth.OAuth2;
using Entities;
using System;
using System.Threading.Tasks;

namespace Scripts.Produccion
{
    public class InsertarDatosProduccion
    {
        public static async Task Main(string[] args)
        {
            try
            {
                // Inicializar Firestore con credenciales
                GoogleCredential credential = await GoogleCredential.GetApplicationDefaultAsync();
                FirestoreDb db = new FirestoreDbBuilder { ProjectId = "muebleria-la-plata", DatabaseId = "muebleria", Credential = credential }.Build();
                
                Console.WriteLine("=====================================================");
                Console.WriteLine("Script de Inserciones para Muebleria La Plata");
                Console.WriteLine("=====================================================");
                Console.WriteLine();

                // 1. Insertar Tipos de Comprobantes AFIP
                await InsertarTiposComprobantes(db);
                Console.WriteLine();

                // 2. Insertar Configuración AFIP
                await InsertarConfiguracionAFIP(db);
                Console.WriteLine();

                // 3. Insertar Configuración de Desarrollo
                await InsertarConfiguracionDesarrollo(db);
                Console.WriteLine();

                // 4. Insertar Configuración TusFacturasAPP
                await InsertarConfiguracionTusFacturasAPP(db);
                Console.WriteLine();

                // 5. Insertar Estados de Factura
                await InsertarEstadosFactura(db);
                Console.WriteLine();

                Console.WriteLine("=====================================================");
                Console.WriteLine("Proceso completado exitosamente");
                Console.WriteLine("=====================================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
            }
        }

        private static async Task InsertarTiposComprobantes(FirestoreDb db)
        {
            try
            {
                CollectionReference collection = db.Collection("tipocomprobantes");
                Console.WriteLine("1. Insertando Tipos de Comprobantes AFIP...");

                var tiposComprobantes = new[]
                {
                    // FACTURAS
                    new TipoComprobante { Codigo = "FACTURA_A", Nombre = "Factura A", Abreviatura = "FA", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "FACTURA_B", Nombre = "Factura B", Abreviatura = "FB", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "FACTURA_C", Nombre = "Factura C", Abreviatura = "FC", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "FACTURA_E", Nombre = "Factura E", Abreviatura = "FE", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "FACTURA_M", Nombre = "Factura M", Abreviatura = "FM", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // FACTURAS MIPYME
                    new TipoComprobante { Codigo = "FACTURA_MIPYME_A", Nombre = "Factura MiPyme A", Abreviatura = "FMA", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "FACTURA_MIPYME_B", Nombre = "Factura MiPyme B", Abreviatura = "FMB", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // NOTAS DE CRÉDITO
                    new TipoComprobante { Codigo = "NOTA_CREDITO_A", Nombre = "Nota de Crédito A", Abreviatura = "NCA", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "NOTA_CREDITO_B", Nombre = "Nota de Crédito B", Abreviatura = "NCB", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "NOTA_CREDITO_C", Nombre = "Nota de Crédito C", Abreviatura = "NCC", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "NOTA_CREDITO_E", Nombre = "Nota de Crédito E", Abreviatura = "NCE", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "NOTA_CREDITO_MIPYME_A", Nombre = "Nota de Crédito MiPyme A", Abreviatura = "NCMA", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // NOTAS DE DÉBITO
                    new TipoComprobante { Codigo = "NOTA_DEBITO_A", Nombre = "Nota de Débito A", Abreviatura = "NDA", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "NOTA_DEBITO_B", Nombre = "Nota de Débito B", Abreviatura = "NDB", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "NOTA_DEBITO_C", Nombre = "Nota de Débito C", Abreviatura = "NDC", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "NOTA_DEBITO_E", Nombre = "Nota de Débito E", Abreviatura = "NDE", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // TICKET
                    new TipoComprobante { Codigo = "TICKET", Nombre = "Ticket", Abreviatura = "T", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "TICKET_FACTURA_B", Nombre = "Ticket Factura B", Abreviatura = "TFB", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // COMPROBANTES ESPECIALES
                    new TipoComprobante { Codigo = "RECIBO", Nombre = "Recibo", Abreviatura = "R", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "PRESUPUESTO", Nombre = "Presupuesto", Abreviatura = "P", Signo = 0, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Codigo = "REMITO", Nombre = "Remito", Abreviatura = "RE", Signo = 0, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
                };

                int insertados = 0;
                int existentes = 0;

                foreach (var tipo in tiposComprobantes)
                {
                    try
                    {
                        // Verificar si ya existe un tipo con el mismo código
                        var query = collection.WhereEqualTo("Codigo", tipo.Codigo);
                        var snapshot = await query.GetSnapshotAsync();
                        
                        if (snapshot.Count > 0)
                        {
                            Console.WriteLine($"⚠️  Ya existe: {tipo.Nombre} ({tipo.Abreviatura})");
                            existentes++;
                        }
                        else
                        {
                            // Agregar el documento y obtener el ID generado automáticamente
                            DocumentReference docRef = await collection.AddAsync(tipo);
                            Console.WriteLine($"✓ Insertado: {tipo.Nombre} ({tipo.Abreviatura}) - ID: {docRef.Id}");
                            insertados++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error procesando {tipo.Nombre}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {insertados} insertados, {existentes} ya existían de {tiposComprobantes.Length} tipos de comprobantes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error en tipos de comprobantes: {ex.Message}");
            }
        }

        private static async Task InsertarConfiguracionAFIP(FirestoreDb db)
        {
            try
            {
                CollectionReference collection = db.Collection("configuracions");
                Console.WriteLine("2. Insertando Configuración AFIP...");

                var configuraciones = new[]
                {
                    new Configuracion { Codigo = "EMPRESA_CUIT", Clave = "EMPRESA_CUIT", Valor = "20398015003", Descripcion = "CUIT de la empresa para facturación AFIP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "EMPRESA_RAZON_SOCIAL", Clave = "EMPRESA_RAZON_SOCIAL", Valor = "RODRIGO EMANUEL NOVOA", Descripcion = "Razón social de la empresa", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "EMPRESA_DOMICILIO", Clave = "EMPRESA_DOMICILIO", Valor = "12 ENTRE 61 Y 62 1410, LA PLATA SUDESTE CALLE 50 AMBAS VEREDAS (CP: 1900), BUENOS AIRES", Descripcion = "Domicilio fiscal de la empresa", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "PUNTO_VENTA_DEFAULT", Clave = "PUNTO_VENTA_DEFAULT", Valor = "00007", Descripcion = "Punto de venta por defecto para facturación", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "CONDICION_IVA_EMPRESA", Clave = "CONDICION_IVA_EMPRESA", Valor = "1", Descripcion = "Condición IVA de la empresa (1=Responsable Inscripto)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
                };

                int insertados = 0;
                int existentes = 0;

                foreach (var config in configuraciones)
                {
                    try
                    {
                        // Verificar si ya existe una configuración con el mismo código
                        var query = collection.WhereEqualTo("Codigo", config.Codigo);
                        var snapshot = await query.GetSnapshotAsync();
                        
                        if (snapshot.Count > 0)
                        {
                            Console.WriteLine($"⚠️  Ya existe: {config.Clave} = {config.Valor}");
                            existentes++;
                        }
                        else
                        {
                            // Agregar el documento y obtener el ID generado automáticamente
                            DocumentReference docRef = await collection.AddAsync(config);
                            Console.WriteLine($"✓ Insertado: {config.Clave} = {config.Valor} - ID: {docRef.Id}");
                            insertados++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error procesando {config.Clave}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {insertados} insertados, {existentes} ya existían de {configuraciones.Length} configuraciones AFIP");
                Console.WriteLine("⚠️  IMPORTANTE: Recuerda reemplazar los valores 'REEMPLAZAR_CON_*' con los datos reales de tu empresa.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error en configuración AFIP: {ex.Message}");
            }
        }

        private static async Task InsertarConfiguracionDesarrollo(FirestoreDb db)
        {
            try
            {
                CollectionReference collection = db.Collection("configuracions");
                Console.WriteLine("3. Insertando Configuración de Desarrollo...");

                var configuraciones = new[]
                {
                    // Configuración de credenciales de prueba para TusFacturasAPP
                    new Configuracion { Codigo = "TUSFACTURAS_USER_TOKEN_DEV", Clave = "TUSFACTURAS_USER_TOKEN", Valor = "TOKEN_DE_DESARROLLO", Descripcion = "Token de usuario para TusFacturasAPP (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "TUSFACTURAS_API_KEY_DEV", Clave = "TUSFACTURAS_API_KEY", Valor = "API_KEY_DE_DESARROLLO", Descripcion = "API Key para TusFacturasAPP (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "TUSFACTURAS_API_TOKEN_DEV", Clave = "TUSFACTURAS_API_TOKEN", Valor = "API_TOKEN_DE_DESARROLLO", Descripcion = "API Token para TusFacturasAPP (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "TUSFACTURAS_BASE_URL_DEV", Clave = "TUSFACTURAS_BASE_URL", Valor = "https://www.tusfacturas.app/app/api/v2", Descripcion = "URL base de la API de TusFacturasAPP (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // Configuración de datos de prueba para AFIP
                    new Configuracion { Codigo = "EMPRESA_CUIT_DEV", Clave = "EMPRESA_CUIT", Valor = "20123456789", Descripcion = "CUIT de prueba para desarrollo", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "EMPRESA_RAZON_SOCIAL_DEV", Clave = "EMPRESA_RAZON_SOCIAL", Valor = "EMPRESA DE PRUEBA S.A.", Descripcion = "Razón social de prueba", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "EMPRESA_DOMICILIO_DEV", Clave = "EMPRESA_DOMICILIO", Valor = "Calle Falsa 123, La Plata, Buenos Aires", Descripcion = "Domicilio de prueba", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "PUNTO_VENTA_DEFAULT_DEV", Clave = "PUNTO_VENTA_DEFAULT", Valor = "0001", Descripcion = "Punto de venta por defecto (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "CONDICION_IVA_EMPRESA_DEV", Clave = "CONDICION_IVA_EMPRESA", Valor = "1", Descripcion = "Condición IVA de prueba (1=Responsable Inscripto)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
                };

                int insertados = 0;
                int existentes = 0;

                foreach (var config in configuraciones)
                {
                    try
                    {
                        // Verificar si ya existe una configuración con el mismo código
                        var query = collection.WhereEqualTo("Codigo", config.Codigo);
                        var snapshot = await query.GetSnapshotAsync();
                        
                        if (snapshot.Count > 0)
                        {
                            Console.WriteLine($"⚠️  Ya existe: {config.Clave} = {config.Valor}");
                            existentes++;
                        }
                        else
                        {
                            // Agregar el documento y obtener el ID generado automáticamente
                            DocumentReference docRef = await collection.AddAsync(config);
                            Console.WriteLine($"✓ Insertado: {config.Clave} = {config.Valor} - ID: {docRef.Id}");
                            insertados++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error procesando {config.Clave}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {insertados} insertados, {existentes} ya existían de {configuraciones.Length} configuraciones de desarrollo");
                Console.WriteLine("ℹ️  NOTA: Este script es SOLO para desarrollo/testing. Los valores son de prueba y NO funcionarán en producción.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error en configuración de desarrollo: {ex.Message}");
            }
        }

        private static async Task InsertarConfiguracionTusFacturasAPP(FirestoreDb db)
        {
            try
            {
                CollectionReference collection = db.Collection("configuracions");
                Console.WriteLine("4. Insertando Configuración TusFacturasAPP...");

                var configuraciones = new[]
                {
                    new Configuracion { Codigo = "TUSFACTURAS_USER_TOKEN", Clave = "TUSFACTURAS_USER_TOKEN", Valor = "762e5d8949b2d48a241ccedcc2364b85512cb2203dc81e452ac485b4f47139c6", Descripcion = "Token de usuario para TusFacturasAPP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "TUSFACTURAS_API_KEY", Clave = "TUSFACTURAS_API_KEY", Valor = "68658", Descripcion = "API Key para TusFacturasAPP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "TUSFACTURAS_API_TOKEN", Clave = "TUSFACTURAS_API_TOKEN", Valor = "ffdef45e8f57f0c90e737a289408a014", Descripcion = "API Token para TusFacturasAPP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Codigo = "TUSFACTURAS_BASE_URL", Clave = "TUSFACTURAS_BASE_URL", Valor = "https://www.tusfacturas.app/app/api/v2", Descripcion = "URL base de la API de TusFacturasAPP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
                };

                // Verificar si hay valores placeholder
                bool hayPlaceholders = false;
                foreach (var config in configuraciones)
                {
                    if (config.Valor.Contains("REEMPLAZAR_CON_"))
                    {
                        hayPlaceholders = true;
                        break;
                    }
                }

                if (hayPlaceholders)
                {
                    Console.WriteLine("⚠️  ADVERTENCIA: Este script contiene valores placeholder que deben ser reemplazados.");
                    Console.WriteLine("Por favor, proporciona las credenciales reales de TusFacturasAPP:");
                    Console.WriteLine();
                    Console.WriteLine("1. TUSFACTURAS_USER_TOKEN");
                    Console.WriteLine("2. TUSFACTURAS_API_KEY");
                    Console.WriteLine("3. TUSFACTURAS_API_TOKEN");
                    Console.WriteLine();
                    Console.WriteLine("Una vez que tengas estos valores, edita este archivo y reemplaza los valores 'REEMPLAZAR_CON_*_REAL'");
                    Console.WriteLine("con las credenciales reales antes de ejecutar este script.");
                    Console.WriteLine("⚠️  Saltando inserción de configuración TusFacturasAPP hasta obtener credenciales reales.");
                    return;
                }

                int insertados = 0;
                int existentes = 0;

                foreach (var config in configuraciones)
                {
                    try
                    {
                        // Verificar si ya existe una configuración con el mismo código
                        var query = collection.WhereEqualTo("Codigo", config.Codigo);
                        var snapshot = await query.GetSnapshotAsync();
                        
                        if (snapshot.Count > 0)
                        {
                            Console.WriteLine($"⚠️  Ya existe: {config.Clave} = {config.Valor.Substring(0, Math.Min(20, config.Valor.Length))}...");
                            existentes++;
                        }
                        else
                        {
                            // Agregar el documento y obtener el ID generado automáticamente
                            DocumentReference docRef = await collection.AddAsync(config);
                            Console.WriteLine($"✓ Insertado: {config.Clave} = {config.Valor.Substring(0, Math.Min(20, config.Valor.Length))}... - ID: {docRef.Id}");
                            insertados++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error procesando {config.Clave}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {insertados} insertados, {existentes} ya existían de {configuraciones.Length} configuraciones de TusFacturasAPP");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error en configuración TusFacturasAPP: {ex.Message}");
            }
        }

        private static async Task InsertarEstadosFactura(FirestoreDb db)
        {
            try
            {
                CollectionReference collection = db.Collection("estadofacturas");
                Console.WriteLine("5. Insertando Estados de Factura...");

                var estadosFactura = new[]
                {
                    // Estados para Facturas
                    new EstadoFactura { Nombre = "Factura Emitida", Descripcion = "Factura emitida correctamente y válida", Codigo = "EMITIDA", PermiteAnulacion = true, EsEstadoFinal = false, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new EstadoFactura { Nombre = "Factura Pendiente", Descripcion = "Factura en proceso de emisión", Codigo = "PENDIENTE", PermiteAnulacion = true, EsEstadoFinal = false, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new EstadoFactura { Nombre = "Error en Emisión", Descripcion = "Error durante la emisión de la factura", Codigo = "ERROR", PermiteAnulacion = false, EsEstadoFinal = false, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new EstadoFactura { Nombre = "Factura Anulada", Descripcion = "Factura anulada por el usuario", Codigo = "ANULADA", PermiteAnulacion = false, EsEstadoFinal = true, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // Estados para Notas de Crédito
                    new EstadoFactura { Nombre = "Nota de Crédito Emitida", Descripcion = "Nota de crédito emitida correctamente", Codigo = "NOTA_CREDITO_EMITIDA", PermiteAnulacion = false, EsEstadoFinal = true, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new EstadoFactura { Nombre = "Nota de Crédito Pendiente", Descripcion = "Nota de crédito en proceso de emisión", Codigo = "NOTA_CREDITO_PENDIENTE", PermiteAnulacion = true, EsEstadoFinal = false, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new EstadoFactura { Nombre = "Error en Nota de Crédito", Descripcion = "Error durante la emisión de la nota de crédito", Codigo = "NOTA_CREDITO_ERROR", PermiteAnulacion = false, EsEstadoFinal = false, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // Estados para Notas de Débito
                    new EstadoFactura { Nombre = "Nota de Débito Emitida", Descripcion = "Nota de débito emitida correctamente", Codigo = "NOTA_DEBITO_EMITIDA", PermiteAnulacion = false, EsEstadoFinal = true, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new EstadoFactura { Nombre = "Nota de Débito Pendiente", Descripcion = "Nota de débito en proceso de emisión", Codigo = "NOTA_DEBITO_PENDIENTE", PermiteAnulacion = true, EsEstadoFinal = false, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new EstadoFactura { Nombre = "Error en Nota de Débito", Descripcion = "Error durante la emisión de la nota de débito", Codigo = "NOTA_DEBITO_ERROR", PermiteAnulacion = false, EsEstadoFinal = false, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // Estados para Comprobantes No Fiscales
                    new EstadoFactura { Nombre = "Remito Emitido", Descripcion = "Remito emitido correctamente", Codigo = "REMITO_EMITIDO", PermiteAnulacion = false, EsEstadoFinal = true, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new EstadoFactura { Nombre = "Presupuesto Emitido", Descripcion = "Presupuesto emitido correctamente", Codigo = "PRESUPUESTO_EMITIDO", PermiteAnulacion = false, EsEstadoFinal = true, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
                };

                int insertados = 0;
                int existentes = 0;

                foreach (var estado in estadosFactura)
                {
                    try
                    {
                        // Verificar si ya existe un estado con el mismo código
                        var query = collection.WhereEqualTo("Codigo", estado.Codigo);
                        var snapshot = await query.GetSnapshotAsync();
                        
                        if (snapshot.Count > 0)
                        {
                            Console.WriteLine($"⚠️  Ya existe: {estado.Nombre} ({estado.Codigo})");
                            existentes++;
                        }
                        else
                        {
                            // Agregar el documento y obtener el ID generado automáticamente
                            DocumentReference docRef = await collection.AddAsync(estado);
                            Console.WriteLine($"✓ Insertado: {estado.Nombre} ({estado.Codigo}) - ID: {docRef.Id}");
                            insertados++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error procesando {estado.Nombre}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {insertados} insertados, {existentes} ya existían de {estadosFactura.Length} estados de factura");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error en estados de factura: {ex.Message}");
            }
        }
    }
}
