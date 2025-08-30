using Data;
using Entities;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly FirestoreDb _firestoreDb;

        public SyncController(ApplicationContext context)
        {
            _context = context;
            _firestoreDb = FirestoreDb.Create("YOUR_PROJECT_ID");
        }

        [HttpPost]
        public async Task<IActionResult> Sync()
        {
            await SyncCollection<Cliente>(_context.Clientes, "clientes");
            await SyncCollection<Producto>(_context.Productos, "productos");
            await SyncCollection<Categoria>(_context.Categorias, "categorias");

            return Ok();
        }

        private async Task SyncCollection<T>(DbSet<T> dbSet, string collectionName) where T : class
        {
            var collection = _firestoreDb.Collection(collectionName);
            var snapshot = await collection.GetSnapshotAsync();
            var firestoreData = snapshot.Documents.Select(d => d.ConvertTo<T>()).ToList();

            var localData = await dbSet.ToListAsync();

            foreach (var item in firestoreData)
            {
                if (!localData.Contains(item))
                {
                    await dbSet.AddAsync(item);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
