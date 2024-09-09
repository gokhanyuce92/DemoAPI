using Demo.DTOs;
using Demo.DTOs.Role;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }

        public async Task<CreateRoleResponseDTO> CreateRoleAsync(CreateRoleRequestDTO createRoleRequestDTO)
        {
            if (string.IsNullOrEmpty(createRoleRequestDTO.RoleName))
            {
                return new CreateRoleResponseDTO { IsSuccess = false, ErrorMessage = "Rol adı boş olamaz." };
            }
            var identityRole = new IdentityRole
            {
                Name = createRoleRequestDTO.RoleName
            };

            var result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                return new CreateRoleResponseDTO { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }

            return new CreateRoleResponseDTO { IsSuccess = true, Data = createRoleRequestDTO };
        }

        public ICollection<GetRolesResponseDTO> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            var getRolesResponses = roles.Select(x => new GetRolesResponseDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return getRolesResponses;
        }

        public async Task<UpdateRoleResponseDTO> UpdateRoleAsync(UpdateRoleRequestDTO updateRoleRequestDTO)
        {
            if (string.IsNullOrEmpty(updateRoleRequestDTO.RoleName))
            {
                return new UpdateRoleResponseDTO { IsSuccess = false, ErrorMessage = "Rol adı boş olamaz." };
            }

            var role = await _roleManager.FindByIdAsync(updateRoleRequestDTO.RoleId);
            if (role == null)
            {
                return new UpdateRoleResponseDTO { IsSuccess = false, ErrorMessage = "Rol bulunamadı." };
            }

            role.Name = updateRoleRequestDTO.RoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return new UpdateRoleResponseDTO { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }

            return new UpdateRoleResponseDTO { IsSuccess = true, Data = updateRoleRequestDTO };
        }
    }
}