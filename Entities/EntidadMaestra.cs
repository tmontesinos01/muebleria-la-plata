using Google.Cloud.Firestore;
using System;
using Entities.Interfaces;

namespace Entities
{
    public abstract class EntidadMaestra : IEntity
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public DateTime FechaLog { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public string UserLog { get; set; } = "DefaultUser";

        [FirestoreProperty]
        public bool Activo { get; set; } = true;
    }
}
