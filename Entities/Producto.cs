using Google.Cloud.Firestore;
using Entities.Interfaces;

namespace Entities;

[FirestoreData]
public class Producto : IEntity
{
    [FirestoreProperty]
    public string? Id { get; set; }

    [FirestoreProperty]
    public string? Nombre { get; set; }

    [FirestoreProperty]
    public decimal Precio { get; set; }

    [FirestoreProperty]
    public string? CategoriaId { get; set; }
}
