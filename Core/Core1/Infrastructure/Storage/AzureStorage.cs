using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core1.Infrastructure.Storage
{
    public class AzureStorage : IWebStorage
    {
        private readonly CloudStorageAccount _account;
        private readonly CloudBlobClient _blobClient;
        private readonly IConfiguration _configuration;

        public AzureStorage(IConfiguration configuration)
        {
            _configuration = configuration;

            var storageConnectionString = _configuration["StorageConnectionString"];

            if (CloudStorageAccount.TryParse(storageConnectionString, out _account))
            {
                _blobClient = _account.CreateCloudBlobClient();
            }
        }

        public async Task<UploadResult> Upload(Stream source)
        {
            return await Upload(source, "default", Guid.NewGuid().ToString());
        }

        private async Task<CloudBlobContainer> GetContainer(string containerName)
        {
            var defaultPermissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            return await GetContainer(containerName, defaultPermissions);
        }

        private async Task<CloudBlobContainer> GetContainer(string containerName, BlobContainerPermissions permissions)
        {
            var defaultContainer = _blobClient.GetContainerReference(containerName);

            await defaultContainer.CreateIfNotExistsAsync();
            await defaultContainer.SetPermissionsAsync(permissions);

            return defaultContainer;
        }

        private async Task<UploadResult> Upload(Stream source, string containerName, string targetPath)
        {
            var container = await GetContainer(containerName);
            var blockBlob = container.GetBlockBlobReference(targetPath);

            await blockBlob.UploadFromStreamAsync(source);

            return new UploadResult
            {
                Url = blockBlob.Uri.AbsoluteUri
            };
        }
    }
}