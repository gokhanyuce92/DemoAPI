using Demo.DTOs;
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

        public async Task<Result<List<string>>> GetUserRolesAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new Result<List<string>> { IsSuccess = false, ErrorMessage = "Kullanıcı bulunamadı." };
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                return new Result<List<string>> { IsSuccess = false, ErrorMessage = "Kullanıcıya ait rol bulunamadı." };
            }
            return new Result<List<string>> { IsSuccess = true, Data = roles.ToList() };
        }

        public async Task<Result<UserRoleDto>> AddUserToRoleAsync(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userRoleDto.UserId);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userRoleDto.UserName);
            }
            if (user == null)
            {
                return new Result<UserRoleDto> { IsSuccess = false, ErrorMessage = "Kullanıcı bulunamadı." };
            }

            var roleExists = await _roleManager.RoleExistsAsync(userRoleDto.RoleName);
            if (!roleExists)
            {
                return new Result<UserRoleDto> { IsSuccess = false, ErrorMessage = "Rol bulunamadı." };
            }

            var result = await _userManager.AddToRoleAsync(user, userRoleDto.RoleName);
            if (!result.Succeeded)
            {
                return new Result<UserRoleDto> { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }

            return new Result<UserRoleDto> { IsSuccess = true, Data = userRoleDto };
        }

        public async Task<Result<UserRoleDto>> RemoveUserFromRoleAsync(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userRoleDto.UserId);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userRoleDto.UserName);
            }
            if (user == null)
            {
                return new Result<UserRoleDto> { IsSuccess = false, ErrorMessage = "Kullanıcı bulunamadı." };
            }

            var roleExists = await _roleManager.RoleExistsAsync(userRoleDto.RoleName);
            if (!roleExists)
            {
                return new Result<UserRoleDto> { IsSuccess = false, ErrorMessage = "Rol bulunamadı." };
            }
            var result = await _userManager.RemoveFromRoleAsync(user, userRoleDto.RoleName);
            if (!result.Succeeded)
            {
                return new Result<UserRoleDto> { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }
            return new Result<UserRoleDto> { IsSuccess = true, Data = userRoleDto };
        }
    }
}