using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using CoreLib;

namespace Api
{
    public class SemaphoreDemo
    {
        public static async Task RunDemo()
        {
            var mux = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            var db = mux.GetDatabase();
            var semaphore = new RedisDistributedSemaphore(db);
            string key = "demo";
            int maxCount = 2;
            Console.WriteLine("Acquiring semaphore...");
            bool acquired = await semaphore.AcquireAsync(key, maxCount);
            Console.WriteLine($"Semaphore acquired: {acquired}");
            if (acquired)
            {
                Console.WriteLine("Releasing semaphore...");
                await semaphore.ReleaseAsync(key);
                Console.WriteLine("Semaphore released.");
            }
        }
    }
}
