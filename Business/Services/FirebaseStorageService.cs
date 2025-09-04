using Business.Interfaces;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Business.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName = "muebleria-la-plata.appspot.com"; // This should be in config

        public FirebaseStorageService(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string destinationPath)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0; 

                var blob = await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: destinationPath,
                    contentType: file.ContentType,
                    source: memoryStream
                );

                return $"https://storage.googleapis.com/{_bucketName}/{destinationPath}";
            }
        }

        public async Task<byte[]> DownloadFileAsync(string filePath)
        {
            using (var memoryStream = new MemoryStream())
            {
                await _storageClient.DownloadObjectAsync(_bucketName, filePath, memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, filePath);
            }
            catch (Google.GoogleApiException e) when (e.Error.Code == 404)
            {
                // If the file doesn't exist, it's already "deleted". We can ignore this error.
            }
        }
    }
}
