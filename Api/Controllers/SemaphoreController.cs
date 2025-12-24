using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using CoreLib;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemaphoreController : ControllerBase
    {
        private readonly IDatabase _db;
        private readonly RedisDistributedSemaphore _semaphore;
        public SemaphoreController()
        {
            var mux = ConnectionMultiplexer.Connect("localhost:6379");
            _db = mux.GetDatabase();
            _semaphore = new RedisDistributedSemaphore(_db);
        }

        [HttpPost("acquire")]
        public async Task<IActionResult> Acquire([FromQuery] string key, [FromQuery] int maxCount)
        {
            var acquired = await _semaphore.AcquireAsync(key, maxCount);
            return Ok(new { acquired });
        }

        [HttpPost("release")]
        public async Task<IActionResult> Release([FromQuery] string key)
        {
            await _semaphore.ReleaseAsync(key);
            return Ok();
        }
    }
}
