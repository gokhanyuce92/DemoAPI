using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService authService;
        private readonly IRedisCacheService redisCacheService;

        public AuthController(ILogger<AuthController> logger, 
        IAuthService authService, 
        IRedisCacheService redisCacheService)
        {
            this._logger = logger;
            this.authService = authService;
            this.redisCacheService = redisCacheService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            var result = await authService.LoginUserAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogWarning(result.ErrorMessage);

                return BadRequest(result.ErrorMessage);
            }
            
            redisCacheService.SetValueAsync("authToken", result.Data.AuthToken, TimeSpan.FromMinutes(30));
            
            return Ok(result.Data);
        }
    }
}