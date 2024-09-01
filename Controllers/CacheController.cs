using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;
        public CacheController(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        [HttpGet("{key}")]
        [Authorize]
        public async Task<IActionResult> Get(string key)
        {
            var value = await _redisCacheService.GetValueAsync(key);
            if (value == null)
            {
                return NotFound(); 
            }
            return Ok(value);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Set([FromBody] RedisCacheRequestModel redisCacheRequestModel)
        {
            bool success = await _redisCacheService.SetValueAsync(redisCacheRequestModel.Key, redisCacheRequestModel.Value, TimeSpan.FromHours(1));
            if (!success)
            {
                return BadRequest(new ErrorResponse { ErrorCode = 1001, ErrorMessage = "Redis'e yazma işlemi başarısız oldu." });
            }
            return Ok();
        }

        [HttpDelete("{key}")]
        [Authorize]
        public async Task<IActionResult> Delete(string key)
        {
            await _redisCacheService.Clear(key);
            return Ok();
        }
    }
}