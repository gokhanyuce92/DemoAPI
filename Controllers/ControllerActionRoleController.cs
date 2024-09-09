using AutoMapper;
using Demo.DTOs.ControllerActionRole;
using Demo.Entities;
using Demo.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerActionRoleController : ControllerBase
    {
        private readonly IControllerActionRoleService _controllerActionRoleService;
        public ControllerActionRoleController(IControllerActionRoleService controllerActionRoleService)
        {
            _controllerActionRoleService = controllerActionRoleService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddControllerActionRoleRequestDTO addControllerActionRoleDTO)
        {
            try
            {
                await _controllerActionRoleService.AddAsync(addControllerActionRoleDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("ControllerActionRole added successfully");
        }
    }
}