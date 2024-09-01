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
        private readonly IAuthService authService;
        private readonly IRedisCacheService redisCacheService;

        public AuthController(IAuthService authService, IRedisCacheService redisCacheService)
        {
            this.authService = authService;
            this.redisCacheService = redisCacheService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            var result = await authService.LoginUserAsync(request);
            if (!result.AuthenticateResult)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı");
            }
            
            redisCacheService.SetValueAsync("authToken", result.AuthToken, TimeSpan.FromMinutes(30));
            return Ok(result);
        }
    }
}