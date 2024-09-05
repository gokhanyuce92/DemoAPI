using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserLoginRequest user)
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