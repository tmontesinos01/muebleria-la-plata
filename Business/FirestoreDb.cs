using Google.Cloud.Firestore;

namespace Business
{
    public class FirestoreDb
    {
        private readonly Google.Cloud.Firestore.FirestoreDb _db;

        public FirestoreDb(string projectId)
        {
            _db = Google.Cloud.Firestore.FirestoreDb.Create(projectId);
        }

        public CollectionReference Collection(string collectionName)
        {
            return _db.Collection(collectionName);
        }
    }
}
