using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace CoreLib
{
    public class RedisDistributedSemaphore : IDistributedSemaphore
    {
        private readonly IDatabase _db;
        private readonly string _semaphorePrefix = "semaphore:";
        public RedisDistributedSemaphore(IDatabase db)
        {
            _db = db;
        }

        public async Task<bool> AcquireAsync(string key, int maxCount, int timeoutMs = 0)
        {
            var semaphoreKey = _semaphorePrefix + key;
            var token = Guid.NewGuid().ToString();
            var expiry = TimeSpan.FromMilliseconds(timeoutMs > 0 ? timeoutMs : 30000);
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var tran = _db.CreateTransaction();
            tran.AddCondition(Condition.ListLengthLessThan(semaphoreKey, maxCount));
            _ = tran.ListRightPushAsync(semaphoreKey, token);
            _ = tran.KeyExpireAsync(semaphoreKey, expiry);
            bool committed = await tran.ExecuteAsync();
            return committed;
        }

        public async Task ReleaseAsync(string key)
        {
            var semaphoreKey = _semaphorePrefix + key;
            // For simplicity, just pop one element
            await _db.ListLeftPopAsync(semaphoreKey);
        }
    }
}
