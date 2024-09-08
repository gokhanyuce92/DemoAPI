using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Demo.Repositories.Abstract;

namespace Demo.Services
{
    public class ControllerActionRoleService : IControllerActionRoleService
    {
        private readonly IControllerActionRoleRepository _controllerActionRoleRepository;
        public ControllerActionRoleService(IControllerActionRoleRepository controllerActionRoleRepository)
        {
            _controllerActionRoleRepository = controllerActionRoleRepository;
        }

        public async Task<Result<ControllerActionRole>> AddAsync(ControllerActionRole controllerActionRole)
        {
            return await _controllerActionRoleRepository.AddAsync(controllerActionRole);
        }

        public async Task DeleteAsync(int id)
        {
            await _controllerActionRoleRepository.DeleteAsync(id);
        }

    }
}