
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Data.Interfaces;
using Data.Repositories;
using Business.Interfaces;
using Business.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Entities;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            Console.WriteLine("Iniciando proceso de limpieza y sembrado de base de datos...");

            try
            {
                await LimpiarColecciones(services);
                await InsertarDatosProduccion(services);
                Console.WriteLine("\n¡Proceso completado exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                var firestoreProjectId = hostContext.Configuration["Firebase:ProjectId"];
                if (string.IsNullOrEmpty(firestoreProjectId))
                {
                    throw new Exception("El ProjectId de Firebase no está configurado en appsettings.json");
                }
                
                var builder = new FirestoreDbBuilder
                {
                    ProjectId = firestoreProjectId,
                    DatabaseId = "muebleria"
                };
                services.AddSingleton(builder.Build());

                services.AddScoped(typeof(IRepository<>), typeof(FirestoreRepository<>));
                
                // Registrar HttpClient
                services.AddHttpClient();

                // Inyectar todos los servicios de negocio
                services.AddScoped<IConfiguracionBusiness, ConfiguracionBusiness>();
                services.AddScoped<IEstadoFacturaBusiness, EstadoFacturaBusiness>();
                services.AddScoped<ITipoComprobanteBusiness, TipoComprobanteBusiness>();
                services.AddScoped<IPerfilBusiness, PerfilBusiness>();
                services.AddScoped<IUsuarioBusiness, UsuarioBusiness>();
                services.AddScoped<IUnidadMedidaBusiness, UnidadMedidaBusiness>();
                services.AddScoped<ICategoriaBusiness, CategoriaBusiness>();
                services.AddScoped<IProductoBusiness, ProductoBusiness>();
                services.AddScoped<IClienteBusiness, ClienteBusiness>();

                services.AddTransient<InsertarDatosProduccion>();
            });

    public static async Task LimpiarColecciones(IServiceProvider services)
    {
        Console.WriteLine("\nLimpiando colecciones...");
        var db = services.GetRequiredService<FirestoreDb>();

        var colecciones = new List<string>
        {
            "categorias", "clientes", "configuracions", "estadofacturas", 
            "facturas", "movimientostocks", "perfils", "productos", 
            "tipocomprobantes", "unidadmedidas", "usuarios", "ventas", "ventadetalles"
        };

        foreach (var coleccionNombre in colecciones)
        {
            try
            {
                CollectionReference collectionRef = db.Collection(coleccionNombre);
                QuerySnapshot snapshot = await collectionRef.Limit(500).GetSnapshotAsync();
                int count = 0;
                
                while (snapshot.Documents.Count > 0)
                {
                    var batch = db.StartBatch();
                    foreach (var doc in snapshot.Documents)
                    {
                        batch.Delete(doc.Reference);
                        count++;
                    }
                    await batch.CommitAsync();
                    snapshot = await collectionRef.Limit(500).GetSnapshotAsync();
                }
                Console.WriteLine($" -> {count} documentos borrados de '{coleccionNombre}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> No se pudo limpiar la colección '{coleccionNombre}'. Puede que no exista. Error: {ex.Message}");
            }
        }
        Console.WriteLine("Limpieza de colecciones finalizada.");
    }

    public static async Task InsertarDatosProduccion(IServiceProvider services)
    {
        Console.WriteLine("\nIniciando sembrado de datos de producción...");
        var insertador = services.GetRequiredService<InsertarDatosProduccion>();
        await insertador.Ejecutar();
        Console.WriteLine("Sembrado de datos finalizado.");
    }
}

public class InsertarDatosProduccion
{
    private readonly IConfiguracionBusiness _configuracionBusiness;
    private readonly IEstadoFacturaBusiness _estadoFacturaBusiness;
    private readonly ITipoComprobanteBusiness _tipoComprobanteBusiness;
    private readonly IPerfilBusiness _perfilBusiness;
    private readonly IUsuarioBusiness _usuarioBusiness;
    private readonly IUnidadMedidaBusiness _unidadMedidaBusiness;
    private readonly ICategoriaBusiness _categoriaBusiness;
    private readonly IProductoBusiness _productoBusiness;
    private readonly IClienteBusiness _clienteBusiness;

    public InsertarDatosProduccion(
        IConfiguracionBusiness configuracionBusiness,
        IEstadoFacturaBusiness estadoFacturaBusiness,
        ITipoComprobanteBusiness tipoComprobanteBusiness,
        IPerfilBusiness perfilBusiness,
        IUsuarioBusiness usuarioBusiness,
        IUnidadMedidaBusiness unidadMedidaBusiness,
        ICategoriaBusiness categoriaBusiness,
        IProductoBusiness productoBusiness,
        IClienteBusiness clienteBusiness)
    {
        _configuracionBusiness = configuracionBusiness;
        _estadoFacturaBusiness = estadoFacturaBusiness;
        _tipoComprobanteBusiness = tipoComprobanteBusiness;
        _perfilBusiness = perfilBusiness;
        _usuarioBusiness = usuarioBusiness;
        _unidadMedidaBusiness = unidadMedidaBusiness;
        _categoriaBusiness = categoriaBusiness;
        _productoBusiness = productoBusiness;
        _clienteBusiness = clienteBusiness;
    }

    public async Task Ejecutar()
    {
        await InsertarConfiguracion();
        await InsertarEstadosFactura();
        await InsertarTiposComprobante();
        await InsertarPerfiles();
        await InsertarUsuarios();
        await InsertarUnidadesMedida();
        await InsertarCategorias();
        await InsertarProductos();
        await InsertarClientes();
    }

    private async Task InsertarConfiguracion() { /* ... */ }
    private async Task InsertarEstadosFactura() { /* ... */ }
    private async Task InsertarTiposComprobante() { /* ... */ }

    private async Task InsertarPerfiles()
    {
        Console.WriteLine(" -> Insertando perfiles...");
        var perfiles = new List<Perfil>
        {
            new Perfil { Id = "ADMIN", Nombre = "Administrador", UserLog = "sistema" },
            new Perfil { Id = "VENDEDOR", Nombre = "Vendedor", UserLog = "sistema" },
        };
        foreach (var perfil in perfiles) await _perfilBusiness.Add(perfil);
    }

    private async Task InsertarUsuarios()
    {
        Console.WriteLine(" -> Insertando usuarios...");
        var usuarios = new List<Usuario>
        {
            new Usuario { Email = "admin@muebleria.com", Password = "123456", IdPerfil = "ADMIN", Nombre = "Admin", Apellido = "User", UserLog = "sistema" },
        };
        foreach (var usuario in usuarios) await _usuarioBusiness.Add(usuario);
    }

    private async Task InsertarUnidadesMedida()
    {
        Console.WriteLine(" -> Insertando unidades de medida...");
        var unidades = new List<UnidadMedida>
        {
            new UnidadMedida { Id = "UNIDAD", Nombre = "Unidad", Simbolo = "u.", UserLog = "sistema" },
        };
        foreach (var unidad in unidades) await _unidadMedidaBusiness.Add(unidad);
    }

    private async Task InsertarCategorias()
    {
        Console.WriteLine(" -> Insertando categorías...");
        var categorias = new List<Categoria>
        {
            new Categoria { Id = "GENERAL", Nombre = "General", UserLog = "sistema" },
        };
        foreach (var categoria in categorias) await _categoriaBusiness.Add(categoria);
    }

    private async Task InsertarProductos()
    {
        Console.WriteLine(" -> Insertando productos...");
        var productos = new List<Producto>
        {
            new Producto { Codigo = "PROD-001", Nombre = "Producto de Prueba", Descripcion = "Un producto para demostración", Precio = 100, IdCategoria = "GENERAL", IdUnidadMedida = "UNIDAD", UserLog = "sistema" },
        };
        foreach (var producto in productos) await _productoBusiness.Add(producto);
    }

    private async Task InsertarClientes()
    {
        Console.WriteLine(" -> Insertando clientes...");
        var clientes = new List<Cliente>
        {
            new Cliente { TipoDocumento = "DNI", NumeroDocumento = "99999999", Nombre = "Consumidor", Apellido = "Final", UserLog = "sistema" },
        };
        foreach (var cliente in clientes) await _clienteBusiness.Add(cliente);
    }
}