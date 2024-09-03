using Demo.Entities;
using Demo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            var result = await _userService.AddAsync(user);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}