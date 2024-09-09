using Demo.DTOs.Role;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IRoleService
    {
        Task<CreateRoleResponseDTO> CreateRoleAsync(CreateRoleRequestDTO createRoleRequestDTO);
        ICollection<GetRolesResponseDTO> GetRoles();
        Task<UpdateRoleResponseDTO> UpdateRoleAsync(UpdateRoleRequestDTO updateRoleRequestDTO);
    }
}