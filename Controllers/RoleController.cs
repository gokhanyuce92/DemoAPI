using Demo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            this._roleService = roleService;
        }

        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            return Ok(_roleService.GetRoles());
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRoleAsync(string roleName)
        {
            var result = await _roleService.CreateRoleAsync(roleName);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRoleAsync(string roleId, string roleName)
        {
            var result = await _roleService.UpdateRoleAsync(roleId, roleName);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Data);
        }
    }
}