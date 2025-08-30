using Microsoft.EntityFrameworkCore;
using Entities;

namespace Data
{
    public class ApplicationContext : DbContext
    {
        // Constructor para el uso en tiempo de ejecución (Inyección de Dependencias)
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        // Constructor para el uso en tiempo de diseño (Herramientas de EF Core)
        public ApplicationContext()
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Esto se llamará cuando las herramientas usen el constructor sin parámetros
                optionsBuilder.UseSqlite("Data Source=muebleria.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.IdCategoria);
        }
    }
}
