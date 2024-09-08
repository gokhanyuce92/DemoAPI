using Demo.Entities;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IControllerActionRoleService
    {
        Task<Result<ControllerActionRole>> AddAsync(ControllerActionRole controllerActionRole);
        Task DeleteAsync(int id);
    }
}