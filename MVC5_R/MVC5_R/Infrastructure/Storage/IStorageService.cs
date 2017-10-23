using System.Threading.Tasks;

namespace MVC5_R.Infrastructure.Storage
{
    public interface IStorageService
    {
        Task<string> Upload(byte[] data, string path, string contentType, string containerName = ContainerNames.Default);
    }
}