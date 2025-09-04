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
                GoogleCredential credential = GoogleCredential.FromFile("firebase-credentials.json");
                FirestoreDb db = FirestoreDb.Create("muebleria", new FirestoreClientBuilder { Credential = credential }.Build());
                
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
                    new TipoComprobante { Id = "factura_a", Nombre = "Factura A", Abreviatura = "FA", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "factura_b", Nombre = "Factura B", Abreviatura = "FB", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "factura_c", Nombre = "Factura C", Abreviatura = "FC", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "factura_e", Nombre = "Factura E", Abreviatura = "FE", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "factura_m", Nombre = "Factura M", Abreviatura = "FM", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // FACTURAS MIPYME
                    new TipoComprobante { Id = "factura_mipyme_a", Nombre = "Factura MiPyme A", Abreviatura = "FMA", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "factura_mipyme_b", Nombre = "Factura MiPyme B", Abreviatura = "FMB", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // NOTAS DE CRÉDITO
                    new TipoComprobante { Id = "nota_credito_a", Nombre = "Nota de Crédito A", Abreviatura = "NCA", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "nota_credito_b", Nombre = "Nota de Crédito B", Abreviatura = "NCB", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "nota_credito_c", Nombre = "Nota de Crédito C", Abreviatura = "NCC", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "nota_credito_e", Nombre = "Nota de Crédito E", Abreviatura = "NCE", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "nota_credito_mipyme_a", Nombre = "Nota de Crédito MiPyme A", Abreviatura = "NCMA", Signo = -1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // NOTAS DE DÉBITO
                    new TipoComprobante { Id = "nota_debito_a", Nombre = "Nota de Débito A", Abreviatura = "NDA", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "nota_debito_b", Nombre = "Nota de Débito B", Abreviatura = "NDB", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "nota_debito_c", Nombre = "Nota de Débito C", Abreviatura = "NDC", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "nota_debito_e", Nombre = "Nota de Débito E", Abreviatura = "NDE", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // TICKET
                    new TipoComprobante { Id = "ticket", Nombre = "Ticket", Abreviatura = "T", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "ticket_factura_b", Nombre = "Ticket Factura B", Abreviatura = "TFB", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // COMPROBANTES ESPECIALES
                    new TipoComprobante { Id = "recibo", Nombre = "Recibo", Abreviatura = "R", Signo = 1, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "presupuesto", Nombre = "Presupuesto", Abreviatura = "P", Signo = 0, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new TipoComprobante { Id = "remito", Nombre = "Remito", Abreviatura = "RE", Signo = 0, Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
                };

                foreach (var tipo in tiposComprobantes)
                {
                    try
                    {
                        DocumentReference docRef = collection.Document(tipo.Id);
                        await docRef.SetAsync(tipo);
                        Console.WriteLine($"✓ Insertado: {tipo.Nombre} ({tipo.Abreviatura})");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error insertando {tipo.Nombre}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {tiposComprobantes.Length} tipos de comprobantes insertados");
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
                    new Configuracion { Id = "empresa_cuit", Clave = "EMPRESA_CUIT", Valor = "20398015003", Descripcion = "CUIT de la empresa para facturación AFIP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "empresa_razon_social", Clave = "EMPRESA_RAZON_SOCIAL", Valor = "RODRIGO EMANUEL NOVOA", Descripcion = "Razón social de la empresa", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "empresa_domicilio", Clave = "EMPRESA_DOMICILIO", Valor = "12 ENTRE 61 Y 62 1410, LA PLATA SUDESTE CALLE 50 AMBAS VEREDAS (CP: 1900), BUENOS AIRES", Descripcion = "Domicilio fiscal de la empresa", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "punto_venta_default", Clave = "PUNTO_VENTA_DEFAULT", Valor = "00007", Descripcion = "Punto de venta por defecto para facturación", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "condicion_iva_empresa", Clave = "CONDICION_IVA_EMPRESA", Valor = "1", Descripcion = "Condición IVA de la empresa (1=Responsable Inscripto)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
                };

                foreach (var config in configuraciones)
                {
                    try
                    {
                        DocumentReference docRef = collection.Document(config.Id);
                        await docRef.SetAsync(config);
                        Console.WriteLine($"✓ Insertado: {config.Clave} = {config.Valor}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error insertando {config.Clave}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {configuraciones.Length} configuraciones AFIP insertadas");
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
                    new Configuracion { Id = "tusfacturas_user_token_dev", Clave = "TUSFACTURAS_USER_TOKEN", Valor = "TOKEN_DE_DESARROLLO", Descripcion = "Token de usuario para TusFacturasAPP (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "tusfacturas_api_key_dev", Clave = "TUSFACTURAS_API_KEY", Valor = "API_KEY_DE_DESARROLLO", Descripcion = "API Key para TusFacturasAPP (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "tusfacturas_api_token_dev", Clave = "TUSFACTURAS_API_TOKEN", Valor = "API_TOKEN_DE_DESARROLLO", Descripcion = "API Token para TusFacturasAPP (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "tusfacturas_base_url_dev", Clave = "TUSFACTURAS_BASE_URL", Valor = "https://www.tusfacturas.app/app/api/v2", Descripcion = "URL base de la API de TusFacturasAPP (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },

                    // Configuración de datos de prueba para AFIP
                    new Configuracion { Id = "empresa_cuit_dev", Clave = "EMPRESA_CUIT", Valor = "20123456789", Descripcion = "CUIT de prueba para desarrollo", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "empresa_razon_social_dev", Clave = "EMPRESA_RAZON_SOCIAL", Valor = "EMPRESA DE PRUEBA S.A.", Descripcion = "Razón social de prueba", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "empresa_domicilio_dev", Clave = "EMPRESA_DOMICILIO", Valor = "Calle Falsa 123, La Plata, Buenos Aires", Descripcion = "Domicilio de prueba", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "punto_venta_default_dev", Clave = "PUNTO_VENTA_DEFAULT", Valor = "0001", Descripcion = "Punto de venta por defecto (DEV)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "condicion_iva_empresa_dev", Clave = "CONDICION_IVA_EMPRESA", Valor = "1", Descripcion = "Condición IVA de prueba (1=Responsable Inscripto)", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
                };

                foreach (var config in configuraciones)
                {
                    try
                    {
                        DocumentReference docRef = collection.Document(config.Id);
                        await docRef.SetAsync(config);
                        Console.WriteLine($"✓ Insertado: {config.Clave} = {config.Valor}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error insertando {config.Clave}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {configuraciones.Length} configuraciones de desarrollo insertadas");
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
                    new Configuracion { Id = "tusfacturas_user_token", Clave = "TUSFACTURAS_USER_TOKEN", Valor = "762e5d8949b2d48a241ccedcc2364b85512cb2203dc81e452ac485b4f47139c6", Descripcion = "Token de usuario para TusFacturasAPP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "tusfacturas_api_key", Clave = "TUSFACTURAS_API_KEY", Valor = "68658", Descripcion = "API Key para TusFacturasAPP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "tusfacturas_api_token", Clave = "TUSFACTURAS_API_TOKEN", Valor = "ffdef45e8f57f0c90e737a289408a014", Descripcion = "API Token para TusFacturasAPP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" },
                    new Configuracion { Id = "tusfacturas_base_url", Clave = "TUSFACTURAS_BASE_URL", Valor = "https://www.tusfacturas.app/app/api/v2", Descripcion = "URL base de la API de TusFacturasAPP", Activo = true, FechaCreacion = DateTime.UtcNow, FechaLog = DateTime.UtcNow, UserLog = "Sistema" }
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

                foreach (var config in configuraciones)
                {
                    try
                    {
                        DocumentReference docRef = collection.Document(config.Id);
                        await docRef.SetAsync(config);
                        Console.WriteLine($"✓ Insertado: {config.Clave} = {config.Valor.Substring(0, Math.Min(20, config.Valor.Length))}...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Error insertando {config.Clave}: {ex.Message}");
                    }
                }

                Console.WriteLine($"✓ Completado: {configuraciones.Length} configuraciones de TusFacturasAPP insertadas");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error en configuración TusFacturasAPP: {ex.Message}");
            }
        }
    }
}
