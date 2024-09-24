using Demo.DTOs;
using Demo.DTOs.UserRole;
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
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService userRoleService)
        {
            this._userRoleService = userRoleService;
        }

        [HttpPost("GetUserRoles")]
        public async Task<IActionResult> GetUserRolesAsync([FromBody] GetUserRolesRequestDTO getUserRolesRequestDTO)
        {
            var response = await _userRoleService.GetUserRolesAsync(getUserRolesRequestDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// AdminPolicy yetkisine sahip sistem yöneticisi kullanıcıya rol atayabilir.
        /// </summary>
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRoleAsync([FromBody] AddUserRoleRequestDTO addUserRoleRequestDTO)
        {
            var response = await _userRoleService.AddUserRoleAsync(addUserRoleRequestDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// AdminPolicy yetkisine sahip sistem yöneticisi kullanıcıdan rolü geri alabilir.
        /// </summary>
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRoleAsync([FromBody] RemoveUserRoleRequestDTO removeUserRoleRequestDTO)
        {
            var response = await _userRoleService.RemoveUserRoleAsync(removeUserRoleRequestDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}