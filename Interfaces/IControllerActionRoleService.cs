using Demo.DTOs.ControllerActionRole;
using Demo.Entities;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IControllerActionRoleService
    {
        Task AddAsync(AddControllerActionRoleRequestDTO controllerActionRole);
        Task DeleteAsync(int id);
    }
}