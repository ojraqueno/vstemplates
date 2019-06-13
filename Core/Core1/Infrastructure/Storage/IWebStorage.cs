using System.IO;
using System.Threading.Tasks;

namespace Core1.Infrastructure.Storage
{
    public interface IWebStorage
    {
        Task<UploadResult> Upload(Stream source);
    }
}