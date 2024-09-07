using Demo.DTOs;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IUserRoleService
    {
        Task<Result<List<string>>> GetUserRolesAsync(string userName);
        Task<Result<UserRoleDto>> AddUserToRoleAsync(UserRoleDto userRoleDto);
        Task<Result<UserRoleDto>> RemoveUserFromRoleAsync(UserRoleDto userRoleDto);
    }
}