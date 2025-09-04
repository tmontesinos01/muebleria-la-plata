using System;
using Business.Interfaces;
using Business.Services;
using Data.Interfaces;
using Data.Repositories;
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

// Add HttpClient for external API calls
builder.Services.AddHttpClient();

// Add this to make all routes lowercase
builder.Services.AddRouting(options => options.LowercaseUrls = true);

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

GoogleCredential credential = await GoogleCredential.GetApplicationDefaultAsync();

builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = credential,
    ProjectId = firebaseProjectId,
}));

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
builder.Services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
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
builder.Services.AddScoped<IUsuarioBusiness, UsuarioBusiness>();

// Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(FirestoreRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
// Always enable Swagger, regardless of the environment
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Muebleria API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
