using Demo.DTOs;
using Demo.DTOs.UserRole;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IUserRoleService
    {
        Task<GetUserRolesResponseDTO> GetUserRolesAsync(GetUserRolesRequestDTO getUserRolesRequestDTO);
        Task<AddUserRoleResponseDTO> AddUserRoleAsync(AddUserRoleRequestDTO addUserRoleRequestDTO);
        Task<RemoveUserRoleResponseDTO> RemoveUserRoleAsync(RemoveUserRoleRequestDTO removeUserRoleRequestDTO);
    }
}