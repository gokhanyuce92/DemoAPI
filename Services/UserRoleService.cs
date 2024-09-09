using Demo.DTOs;
using Demo.DTOs.UserRole;
using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public UserRoleService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public async Task<GetUserRolesResponseDTO> GetUserRolesAsync(GetUserRolesRequestDTO getUserRolesRequestDTO)
        {
            var user = await _userManager.FindByNameAsync(getUserRolesRequestDTO.UserName);
            if (user == null)
            {
                return new GetUserRolesResponseDTO { IsSuccess = false, ErrorMessage = "Kullanıcı bulunamadı." };
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new GetUserRolesResponseDTO { IsSuccess = true, Data = roles.ToList() };
        }

        public async Task<AddUserRoleResponseDTO> AddUserRoleAsync(AddUserRoleRequestDTO addUserRoleRequestDTO)
        {
            var user = await _userManager.FindByIdAsync(addUserRoleRequestDTO.UserId);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(addUserRoleRequestDTO.UserName);
            }
            if (user == null)
            {
                return new AddUserRoleResponseDTO { IsSuccess = false, ErrorMessage = "Kullanıcı bulunamadı." };
            }

            var roleExists = await _roleManager.RoleExistsAsync(addUserRoleRequestDTO.RoleName);
            if (!roleExists)
            {
                return new AddUserRoleResponseDTO { IsSuccess = false, ErrorMessage = "Rol bulunamadı." };
            }

            var result = await _userManager.AddToRoleAsync(user, addUserRoleRequestDTO.RoleName);
            if (!result.Succeeded)
            {
                return new AddUserRoleResponseDTO { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }

            return new AddUserRoleResponseDTO { IsSuccess = true, Data = addUserRoleRequestDTO };
        }

        public async Task<RemoveUserRoleResponseDTO> RemoveUserRoleAsync(RemoveUserRoleRequestDTO removeUserRoleRequestDTO)
        {
            var user = await _userManager.FindByIdAsync(removeUserRoleRequestDTO.UserId);

            if (user == null)
            {
                return new RemoveUserRoleResponseDTO { IsSuccess = false, ErrorMessage = "Kullanıcı bulunamadı." };
            }

            var roleExists = await _roleManager.RoleExistsAsync(removeUserRoleRequestDTO.RoleName);
            if (!roleExists)
            {
                return new RemoveUserRoleResponseDTO { IsSuccess = false, ErrorMessage = "Rol bulunamadı." };
            }
            var result = await _userManager.RemoveFromRoleAsync(user, removeUserRoleRequestDTO.RoleName);
            if (!result.Succeeded)
            {
                return new RemoveUserRoleResponseDTO { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }
            return new RemoveUserRoleResponseDTO { IsSuccess = true, Data = removeUserRoleRequestDTO };
        }
    }
}