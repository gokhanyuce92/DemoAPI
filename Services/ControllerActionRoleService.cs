using AutoMapper;
using Demo.DTOs.ControllerActionRole;
using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Demo.Repositories.Abstract;

namespace Demo.Services
{
    public class ControllerActionRoleService : IControllerActionRoleService
    {
        private readonly IControllerActionRoleRepository _controllerActionRoleRepository;
        private readonly IMapper _mapper;
        public ControllerActionRoleService(IControllerActionRoleRepository controllerActionRoleRepository, IMapper mapper)
        {
            _controllerActionRoleRepository = controllerActionRoleRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(AddControllerActionRoleRequestDTO controllerActionRole)
        {
            var request = _mapper.Map<ControllerActionRole>(controllerActionRole);
            await _controllerActionRoleRepository.AddAsync(request);
        }

        public async Task DeleteAsync(int id)
        {
            await _controllerActionRoleRepository.DeleteAsync(id);
        }
    }
}