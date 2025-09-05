using System;
using System.Threading.Tasks;

namespace TestProject
{
    /// <summary>
    /// Programa principal para ejecutar el test completo del sistema de facturación
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("🚀 SISTEMA DE PRUEBAS - MUEBLERÍA LA PLATA");
            Console.WriteLine("==========================================");
            Console.WriteLine("Test completo del sistema de facturación electrónica AFIP/ARCA");
            Console.WriteLine("");

            // Configurar URL del servidor
            string baseUrl = "https://localhost:7000";
            if (args.Length > 0)
            {
                baseUrl = args[0];
            }

            Console.WriteLine($"🌐 Conectando a: {baseUrl}");
            Console.WriteLine("");

            using var testSistema = new TestCompletoSistema(baseUrl);

            try
            {
                // Ejecutar test completo
                var resultado = await testSistema.EjecutarTestCompleto();

                // Mostrar resultados
                MostrarResultados(resultado);

                // Mostrar resumen de datos creados
                var resumen = testSistema.ObtenerResumen();
                MostrarResumen(resumen);

                // Mostrar instrucciones para verificar en Firebase Studio
                MostrarInstruccionesFirebase();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR CRÍTICO: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            Console.WriteLine("");
            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        /// <summary>
        /// Muestra los resultados del test
        /// </summary>
        private static void MostrarResultados(TestResult resultado)
        {
            Console.WriteLine("\n📊 RESULTADOS DEL TEST");
            Console.WriteLine("======================");

            if (resultado.Exito)
            {
                Console.WriteLine("✅ ESTADO: EXITOSO");
                Console.WriteLine($"📝 MENSAJE: {resultado.Mensaje}");
            }
            else
            {
                Console.WriteLine("❌ ESTADO: FALLIDO");
                Console.WriteLine($"📝 MENSAJE: {resultado.Mensaje}");
            }

            Console.WriteLine($"🕐 FECHA: {resultado.FechaEjecucion:yyyy-MM-dd HH:mm:ss}");

            if (resultado.Errores.Count > 0)
            {
                Console.WriteLine("\n⚠️ ERRORES ENCONTRADOS:");
                foreach (var error in resultado.Errores)
                {
                    Console.WriteLine($"   • {error}");
                }
            }
        }

        /// <summary>
        /// Muestra el resumen de datos creados
        /// </summary>
        private static void MostrarResumen(TestSummary resumen)
        {
            Console.WriteLine("\n📋 RESUMEN DE DATOS CREADOS");
            Console.WriteLine("============================");

            Console.WriteLine($"📦 Productos creados: {resumen.ProductosCreados}");
            Console.WriteLine($"🛒 Ventas creadas: {resumen.VentasCreadas}");
            Console.WriteLine($"🧾 Facturas emitidas: {resumen.FacturasEmitidas}");
            Console.WriteLine($"🎫 Tickets emitidos: {resumen.TicketsEmitidos}");
            Console.WriteLine($"📝 Notas emitidas: {resumen.NotasEmitidas}");

            if (resumen.FacturasNumeros.Count > 0)
            {
                Console.WriteLine("\n🧾 FACTURAS EMITIDAS:");
                foreach (var factura in resumen.FacturasNumeros)
                {
                    Console.WriteLine($"   • {factura}");
                }
            }

            if (resumen.TicketsNumeros.Count > 0)
            {
                Console.WriteLine("\n🎫 TICKETS EMITIDOS:");
                foreach (var ticket in resumen.TicketsNumeros)
                {
                    Console.WriteLine($"   • {ticket}");
                }
            }

            if (resumen.NotasNumeros.Count > 0)
            {
                Console.WriteLine("\n📝 NOTAS EMITIDAS:");
                foreach (var nota in resumen.NotasNumeros)
                {
                    Console.WriteLine($"   • {nota}");
                }
            }
        }

        /// <summary>
        /// Muestra instrucciones para verificar en Firebase Studio
        /// </summary>
        private static void MostrarInstruccionesFirebase()
        {
            Console.WriteLine("\n🔥 INSTRUCCIONES PARA FIREBASE STUDIO");
            Console.WriteLine("=====================================");
            Console.WriteLine("Para verificar los datos creados en Firebase Studio:");
            Console.WriteLine("");
            Console.WriteLine("1. 🌐 Abrir Firebase Studio en tu navegador");
            Console.WriteLine("2. 📁 Navegar a la colección 'Productos'");
            Console.WriteLine("   - Verificar que se crearon 5 productos de prueba");
            Console.WriteLine("   - Revisar nombres: Mesa de Comedor, Silla de Comedor, etc.");
            Console.WriteLine("");
            Console.WriteLine("3. 👥 Navegar a la colección 'Clientes'");
            Console.WriteLine("   - Verificar que se crearon 3 clientes de prueba");
            Console.WriteLine("   - Revisar: Juan Pérez, María González, Empresa ABC S.A.");
            Console.WriteLine("");
            Console.WriteLine("4. 🛒 Navegar a la colección 'Ventas'");
            Console.WriteLine("   - Verificar que se crearon 2 ventas de prueba");
            Console.WriteLine("   - Revisar detalles y totales");
            Console.WriteLine("");
            Console.WriteLine("5. 🧾 Navegar a la colección 'Facturas'");
            Console.WriteLine("   - Verificar facturas emitidas con CAE");
            Console.WriteLine("   - Revisar tickets emitidos");
            Console.WriteLine("   - Verificar notas de crédito/débito");
            Console.WriteLine("   - Revisar facturas anuladas");
            Console.WriteLine("");
            Console.WriteLine("6. ⚙️ Navegar a la colección 'Configuracion'");
            Console.WriteLine("   - Verificar credenciales de TusFacturasAPP");
            Console.WriteLine("   - Revisar configuración de punto de venta");
            Console.WriteLine("");
            Console.WriteLine("7. 📊 Navegar a la colección 'TipoComprobante'");
            Console.WriteLine("   - Verificar que están todos los tipos configurados");
            Console.WriteLine("   - Revisar: facturas, notas, tickets");
            Console.WriteLine("");
            Console.WriteLine("8. 🔍 Navegar a la colección 'EstadoFactura'");
            Console.WriteLine("   - Verificar estados configurados");
            Console.WriteLine("   - Revisar: EMITIDA, ANULADA, etc.");
            Console.WriteLine("");
            Console.WriteLine("✅ Si todos los datos están presentes, el sistema está funcionando correctamente");
            Console.WriteLine("❌ Si faltan datos, revisar logs del servidor para identificar errores");
        }
    }
}
