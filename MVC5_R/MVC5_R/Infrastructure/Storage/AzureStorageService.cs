using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace MVC5_R.Infrastructure.Storage
{
    public class AzureStorageService : IStorageService
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _blobClient;

        public AzureStorageService()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        public async Task<string> Upload(byte[] data, string path, string contentType, string containerName = ContainerNames.Default)
        {
            var container = GetContainer(containerName);

            var blob = container.GetBlockBlobReference(path);
            blob.Properties.ContentType = contentType;

            await blob.UploadFromByteArrayAsync(data, 0, data.Length);

            return blob.Uri.AbsoluteUri;
        }

        private CloudBlobContainer GetContainer(string containerName)
        {
            var container = _blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return container;
        }
    }
}