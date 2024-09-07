using Demo.DTOs;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IRoleService
    {
        Task<Result<string>> CreateRoleAsync(string roleName);
        ICollection<RoleDto> GetRoles();
        Task<Result<string>> UpdateRoleAsync(string roleId, string roleName);
    }
}