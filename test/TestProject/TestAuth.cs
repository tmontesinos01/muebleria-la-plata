using System;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;

namespace TestProject
{
    public class TestAuth
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== TEST DE AUTENTICACIÓN FIREBASE ===");
                
                // Verificar archivo de credenciales
                var credentialPath = "firebase-credentials.json";
                if (!File.Exists(credentialPath))
                {
                    Console.WriteLine($"❌ No se encontró el archivo: {credentialPath}");
                    return;
                }
                
                Console.WriteLine($"✅ Archivo de credenciales encontrado: {credentialPath}");
                
                // Cargar credenciales
                var credential = GoogleCredential.FromFile(credentialPath);
                Console.WriteLine("✅ Credenciales cargadas correctamente");
                
                // Inicializar Firebase
                FirebaseApp.Create(new AppOptions() { Credential = credential });
                Console.WriteLine("✅ Firebase inicializado correctamente");
                
                // Crear cliente Firestore
                var firestoreDb = FirestoreDb.Create("muebleria-la-plata", new FirestoreClientBuilder { Credential = credential }.Build());
                Console.WriteLine("✅ Cliente Firestore creado correctamente");
                
                // Intentar una operación simple
                Console.WriteLine("🔍 Intentando acceder a la colección 'test'...");
                var testCollection = firestoreDb.Collection("test");
                var testDoc = testCollection.Document("connection-test");
                
                // Intentar leer el documento
                var snapshot = await testDoc.GetSnapshotAsync();
                Console.WriteLine("✅ Conexión a Firestore exitosa!");
                
                if (snapshot.Exists)
                {
                    Console.WriteLine($"📄 Documento encontrado: {snapshot.Id}");
                }
                else
                {
                    Console.WriteLine("📄 Documento no existe (esto es normal)");
                }
                
                Console.WriteLine("\n🎉 ¡AUTENTICACIÓN EXITOSA! Firebase está configurado correctamente.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                Console.WriteLine($"Detalles: {ex}");
                
                if (ex.Message.Contains("Unauthenticated"))
                {
                    Console.WriteLine("\n💡 POSIBLES SOLUCIONES:");
                    Console.WriteLine("1. Verificar que el proyecto 'muebleria-la-plata' existe en Firebase Console");
                    Console.WriteLine("2. Verificar que Firestore está habilitado en el proyecto");
                    Console.WriteLine("3. Verificar que las credenciales tienen permisos para acceder a Firestore");
                    Console.WriteLine("4. Verificar que el archivo de credenciales es válido y no está corrupto");
                }
            }
        }
    }
}
