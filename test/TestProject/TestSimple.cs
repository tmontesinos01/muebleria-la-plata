using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Storage.V1;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Business.Interfaces;
using Business.Services;
using Data.Repositories;
using Data.Interfaces;
using Entities;

namespace TestProject
{
    public class TestSimple
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== TEST SIMPLE DE CONEXIÓN A FIREBASE ===");
                
                // Configurar servicios
                var services = new ServiceCollection();
                ConfigureServices(services);
                var serviceProvider = services.BuildServiceProvider();
                
                // Verificar conexión
                await TestConnection(serviceProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                Console.WriteLine($"Detalles: {ex}");
            }
        }
        
        private static void ConfigureServices(IServiceCollection services)
        {
            // Configuración
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Firebase:ProjectId"] = "muebleria-la-plata",
                    ["Firebase:StorageBucket"] = "muebleria-la-plata.appspot.com"
                })
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Firebase Configuration
            var credentialPath = "firebase-credentials.json";
            if (!File.Exists(credentialPath))
            {
                throw new FileNotFoundException($"No se encontró el archivo de credenciales: {credentialPath}");
            }

            var credential = GoogleCredential.FromFile(credentialPath);
            FirebaseApp.Create(new AppOptions() { Credential = credential });

            // Firestore
            services.AddSingleton<FirestoreDb>(provider =>
                FirestoreDb.Create("muebleria-la-plata", new FirestoreClientBuilder { Credential = credential }.Build()));

            // Google Cloud Storage
            services.AddSingleton(provider => StorageClient.Create(credential));

            // Firebase Storage Service
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();

            // Business Services
            services.AddScoped<ICategoriaBusiness, CategoriaBusiness>();
            services.AddScoped<IProductoBusiness, ProductoBusiness>();

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(FirestoreRepository<>));
        }
        
        private static async Task TestConnection(IServiceProvider serviceProvider)
        {
            Console.WriteLine("🔍 Verificando conexión a Firebase...");
            
            // Verificar que los servicios se pueden crear
            var categoriaBusiness = serviceProvider.GetRequiredService<ICategoriaBusiness>();
            var productoBusiness = serviceProvider.GetRequiredService<IProductoBusiness>();
            
            Console.WriteLine("✅ Servicios creados correctamente");
            
            // Intentar obtener todas las categorías (operación de lectura)
            Console.WriteLine("📖 Intentando leer categorías existentes...");
            var categoriasExistentes = await categoriaBusiness.GetAll();
            Console.WriteLine($"✅ Conexión exitosa! Se encontraron {categoriasExistentes.Count()} categorías");
            
            // Intentar obtener todos los productos (operación de lectura)
            Console.WriteLine("📖 Intentando leer productos existentes...");
            var productosExistentes = await productoBusiness.GetAll();
            Console.WriteLine($"✅ Conexión exitosa! Se encontraron {productosExistentes.Count()} productos");
            
            Console.WriteLine("\n🎉 ¡PRUEBA EXITOSA! El sistema está funcionando correctamente.");
            Console.WriteLine("📊 RESUMEN:");
            Console.WriteLine($"   - Categorías en la base de datos: {categoriasExistentes.Count()}");
            Console.WriteLine($"   - Productos en la base de datos: {productosExistentes.Count()}");
            
            if (categoriasExistentes.Any())
            {
                Console.WriteLine("\n📁 CATEGORÍAS EXISTENTES:");
                foreach (var categoria in categoriasExistentes)
                {
                    Console.WriteLine($"   - {categoria.Nombre} (ID: {categoria.Id})");
                }
            }
            
            if (productosExistentes.Any())
            {
                Console.WriteLine("\n🛋️ PRODUCTOS EXISTENTES:");
                foreach (var producto in productosExistentes)
                {
                    Console.WriteLine($"   - {producto.Nombre} (ID: {producto.Id}) - Categoría: {producto.IdCategoria}");
                }
            }
        }
    }
}
