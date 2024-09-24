using Demo.DTOs.Role;
using Demo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        /// <summary>
        /// AdminPolicy yetkisine sahip sistem yöneticisi yeni bir rol oluşturabilir.
        /// </summary>
        [Authorize(Policy = "AdminPolicy")]
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

        /// <summary>
        /// AdminPolicy yetkisine sahip sistem yöneticisi var olan bir rolü güncelleyebilir.
        /// </summary>
        [Authorize(Policy = "AdminPolicy")]
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