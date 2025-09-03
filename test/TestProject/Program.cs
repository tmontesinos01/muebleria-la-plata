using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

// --- INICIO: Clases de Entidades para el Test ---
// Estas son versiones simplificadas de tus entidades para la (de)serialización en este script de prueba.
public class Categoria
{
    public string? Id { get; set; }
    public string? Nombre { get; set; }
}

public class Producto
{
    public string? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? IdCategoria { get; set; }
    public string? ImagenUrl { get; set; }
    public string? Codigo { get; set; }
    public decimal AlicuotaIva { get; set; }
    public string? IdUnidadMedida { get; set; }
}
// --- FIN: Clases de Entidades para el Test ---

public class TestRunner
{
    public static async Task Main(string[] args)
    {
        // --- CONFIGURACIÓN ---
        // !!! IMPORTANTE: Asegúrate de que esta URL coincida con la dirección donde se está ejecutando tu API.
        const string baseUrl = "http://localhost:5165"; 
        const string testImageName = "test-image.png";
        // La ruta a la imagen de prueba es relativa a la ubicación de ejecución del script.
        // Dado que ejecutaremos desde la carpeta `test`, la ruta es correcta.
        string imagePath = Path.Combine("..", testImageName);

        using var client = new HttpClient();
        client.BaseAddress = new Uri(baseUrl);

        Console.WriteLine("--- INICIANDO PRUEBA DE INTEGRACIÓN DE PRODUCTOS ---");

        try
        {
            // --- PASO 1: CREAR UNA NUEVA CATEGORÍA ---
            Console.WriteLine("\n[Paso 1] Creando una nueva categoría...");
            var nuevaCategoria = new Categoria { Nombre = $"Categoría de Prueba - {Guid.NewGuid()}" };
            var response = await client.PostAsJsonAsync("/categorias", nuevaCategoria);
            response.EnsureSuccessStatusCode();
            var categoriaCreada = await response.Content.ReadFromJsonAsync<Categoria>();
            if (categoriaCreada == null || string.IsNullOrEmpty(categoriaCreada.Id))
            {
                throw new InvalidOperationException("No se pudo crear o deserializar la categoría.");
            }
            Console.WriteLine($"Categoría creada con éxito. ID: {categoriaCreada.Id}");

            // --- PASO 2: CREAR UN NUEVO PRODUCTO CON IMAGEN ---
            Console.WriteLine("\n[Paso 2] Creando un nuevo producto con imagen...");
            using var formData = new MultipartFormDataContent();
            
            // Agregar datos del producto como campos de formulario
            formData.Add(new StringContent($"Producto de Prueba - {Guid.NewGuid()}"), "Nombre");
            formData.Add(new StringContent("Descripción de prueba"), "Descripcion");
            formData.Add(new StringContent("123.45"), "Precio");
            formData.Add(new StringContent("10"), "Stock");
            formData.Add(new StringContent("XYZ-001"), "Codigo");
            formData.Add(new StringContent("21"), "AlicuotaIva");
            formData.Add(new StringContent(categoriaCreada.Id), "IdCategoria");
            // Estos campos son opcionales en el controlador, pero los agregamos para ser completos.
            formData.Add(new StringContent("unidad-default"), "IdUnidadMedida"); // Asumimos un valor por defecto

            // Agregar archivo de imagen
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"No se encontró la imagen de prueba en: {Path.GetFullPath(imagePath)}");
            }
            var imageStream = new StreamContent(File.OpenRead(imagePath));
            imageStream.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            formData.Add(imageStream, "imagen", testImageName);

            response = await client.PostAsync("/productos", formData);
            response.EnsureSuccessStatusCode();
            var productoCreado = await response.Content.ReadFromJsonAsync<Producto>();
            if (productoCreado == null || string.IsNullOrEmpty(productoCreado.Id))
            {
                throw new InvalidOperationException("No se pudo crear o deserializar el producto.");
            }
            Console.WriteLine($"Producto creado con éxito. ID: {productoCreado.Id}");

            // --- PASO 3: RECUPERAR EL PRODUCTO CREADO ---
            Console.WriteLine("\n[Paso 3] Recuperando el producto desde la base de datos...");
            var productoRecuperado = await client.GetFromJsonAsync<Producto>($"/productos/{productoCreado.Id}");
            if (productoRecuperado == null)
            {
                 throw new InvalidOperationException("No se pudo encontrar el producto recién creado.");
            }
            Console.WriteLine("Producto recuperado con éxito.");

            // --- PASO 4: VERIFICAR EL RESULTADO FINAL ---
            Console.WriteLine("\n--- RESULTADO FINAL ---");
            Console.WriteLine(JsonSerializer.Serialize(productoRecuperado, new JsonSerializerOptions { WriteIndented = true }));
            
            if (!string.IsNullOrEmpty(productoRecuperado.ImagenUrl))
            {
                Console.WriteLine("\n\u001b[32mÉXITO: La URL de la imagen se guardó correctamente.\u001b[0m");
            }
            else
            {
                Console.WriteLine("\n\u001b[31mFALLO: La URL de la imagen está vacía.\u001b[0m");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n\u001b[31m--- ERROR EN LA PRUEBA ---");
            Console.WriteLine(ex.ToString());
            Console.WriteLine("\u001b[0m");
        }
        finally
        {
             Console.WriteLine("\n--- PRUEBA DE INTEGRACIÓN FINALIZADA ---");
        }
    }
}
