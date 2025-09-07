using System;
using System.Threading.Tasks;

namespace TestProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var testRunner = new TestCompletoSistema("http://localhost:5165");
            var result = await testRunner.EjecutarTestCompleto();

            if (result.Exito)
            {
                Console.WriteLine("\n✅ PRUEBA DE FACTURACIÓN COMPLETA FINALIZADA CON ÉXITO");
            }
            else
            {
                Console.WriteLine("\n❌ LA PRUEBA DE FACTURACIÓN COMPLETA FALLÓ");
                foreach (var error in result.Errores)
                {
                    Console.WriteLine($"- {error}");
                }
            }

            var summary = testRunner.ObtenerResumen();
            Console.WriteLine("\n--- RESUMEN DE LA PRUEBA ---");
            Console.WriteLine($"- Productos Creados: {summary.ProductosCreados}");
            Console.WriteLine($"- Ventas Simuladas: {summary.VentasCreadas}");
            Console.WriteLine($"- Facturas Emitidas: {summary.FacturasEmitidas}");
            Console.WriteLine($"- Tickets Emitidos: {summary.TicketsEmitidos}");
            Console.WriteLine($"- Notas Emitidas: {summary.NotasEmitidas}");
            Console.WriteLine("---------------------------\n");
        }
    }
}
