using Demo.DTOs;
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
        public async Task<IActionResult> AddAsync(ControllerActionRoleDto controllerActionRole)
        {
            var result = await _controllerActionRoleService.AddAsync(
                new()
                {
                    ControllerName = controllerActionRole.ControllerName,
                    ActionName = controllerActionRole.ActionName,
                    RoleId = controllerActionRole.RoleId
                });
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}