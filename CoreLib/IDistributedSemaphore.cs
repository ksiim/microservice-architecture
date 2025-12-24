using System.Threading.Tasks;

namespace CoreLib
{
    public interface IDistributedSemaphore
    {
        Task<bool> AcquireAsync(string key, int maxCount, int timeoutMs = 0);
        Task ReleaseAsync(string key);
    }
}
