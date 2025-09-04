using Business.Interfaces;
using Business.Services;
using Data.Interfaces;
using Data.Repositorios;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Firebase Configuration
var firebaseProjectId = builder.Configuration["Firebase:ProjectId"];
var credentialsPath = "firebase-credentials.json"; // As per the user's plan

// Create the credential from the file
var credential = GoogleCredential.FromFile(credentialsPath);

// Initialize FirebaseApp with the specific credential
builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = credential,
    ProjectId = firebaseProjectId,
}));

// Initialize FirestoreDb and StorageClient with the same credential
builder.Services.AddSingleton(provider => FirestoreDb.Create(firebaseProjectId, new FirestoreClientBuilder { Credential = credential }.Build()));
builder.Services.AddSingleton(provider => StorageClient.Create(credential));

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/" + firebaseProjectId;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/" + firebaseProjectId,
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true
        };
    });

// Business Services
builder.Services.AddScoped<IProductoBusiness, ProductoBusiness>();
builder.Services.AddScoped<IClienteBusiness, ClienteBusiness>();
builder.Services.AddScoped<ICategoriaBusiness, CategoriaBusiness>();
builder.Services.AddScoped<IImagenService, ImagenService>();
builder.Services.AddScoped<IMovimientoStockBusiness, MovimientoStockBusiness>();
builder.Services.AddScoped<IVentaBusiness, VentaBusiness>();
builder.Services.AddScoped<IVentaDetalleBusiness, VentaDetalleBusiness>();
builder.Services.AddScoped<IFacturacionBusiness, FacturacionBusiness>();
builder.Services.AddScoped<IConfiguracionBusiness, ConfiguracionBusiness>();
builder.Services.AddScoped<IPerfilBusiness, PerfilBusiness>();
builder.Services.AddScoped<ITipoComprobanteBusiness, TipoComprobanteBusiness>();
builder.Services.AddScoped<IUnidadMedidaBusiness, UnidadMedidaBusiness>();


// Repositories
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IMovimientoStockRepositorio, MovimientoStockRepositorio>();
builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();
builder.Services.AddScoped<IVentaDetalleRepositorio, VentaDetalleRepositorio>();
builder.Services.AddScoped<IFacturaRepositorio, FacturaRepositorio>();
builder.Services.AddScoped<IRepositorio<Entities.Configuracion>, RepositorioBase<Entities.Configuracion>> ();
builder.Services.AddScoped<IRepositorio<Entities.Perfil>, RepositorioBase<Entities.Perfil>> ();
builder.Services.AddScoped<IRepositorio<Entities.TipoComprobante>, RepositorioBase<Entities.TipoComprobante>> ();
builder.Services.AddScoped<IRepositorio<Entities.UnidadMedida>, RepositorioBase<Entities.UnidadMedida>> ();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // This line is commented out as it causes issues behind a reverse proxy

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
