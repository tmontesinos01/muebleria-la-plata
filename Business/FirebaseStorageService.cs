using Business.Interfaces;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Business
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public FirebaseStorageService(StorageClient storageClient, IConfiguration configuration)
        {
            _storageClient = storageClient;
            _bucketName = configuration.GetValue<string>("Firebase:StorageBucket");
        }

        public async Task<string> UploadFileAsync(IFormFile file, string destinationPath)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var objectName = destinationPath + "/" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var obj = await _storageClient.UploadObjectAsync(_bucketName, objectName, file.ContentType, memoryStream);
                return $"https://storage.googleapis.com/{_bucketName}/{objectName}";
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
            await _storageClient.DeleteObjectAsync(_bucketName, filePath);
        }
    }
}
