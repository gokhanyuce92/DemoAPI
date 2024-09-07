using Demo.DTOs;
using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService userRoleService)
        {
            this._userRoleService = userRoleService;
        }

        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRolesAsync(string userName)
        {
            var result = await _userRoleService.GetUserRolesAsync(userName);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRoleAsync(UserRoleDto userRoleDto)
        {
            var result = await _userRoleService.AddUserToRoleAsync(userRoleDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRoleAsync(UserRoleDto userRoleDto)
        {
            var result = await _userRoleService.RemoveUserFromRoleAsync(userRoleDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Data);
        }
    }
}