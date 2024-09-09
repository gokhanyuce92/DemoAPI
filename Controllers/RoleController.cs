using Demo.DTOs.Role;
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
        public async Task<IActionResult> CreateRoleAsync(CreateRoleRequestDTO createRoleRequestDTO)
        {
            var response = await _roleService.CreateRoleAsync(createRoleRequestDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRoleAsync(UpdateRoleRequestDTO updateRoleRequestDTO)
        {
            var response = await _roleService.UpdateRoleAsync(updateRoleRequestDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            
            return Ok(response);
        }
    }
}