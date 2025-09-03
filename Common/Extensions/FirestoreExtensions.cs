using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Linq;
using Entities.Interfaces;

namespace Common.Extensions
{
    public static class FirestoreExtensions
    {
        public static T ConvertToWithId<T>(this DocumentSnapshot documentSnapshot) where T : class, IEntity
        {
            if (documentSnapshot.Exists)
            {                
                T obj = documentSnapshot.ConvertTo<T>();
                obj.Id = documentSnapshot.Id;
                return obj;
            }
            return null;
        }

        public static List<T> ConvertAllToWithId<T>(this IReadOnlyList<DocumentSnapshot> snapshots) where T : class, IEntity
        {
            return snapshots.Select(s => s.ConvertToWithId<T>()).ToList();
        }
    }
}
