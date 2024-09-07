using Demo.DTOs;
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

        public async Task<Result<string>> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return new Result<string> { IsSuccess = false, ErrorMessage = "Rol adı boş olamaz." };
            }
            var identityRole = new IdentityRole
            {
                Name = roleName
            };

            var result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                return new Result<string> { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }

            return new Result<string> { IsSuccess = true, Data = "Rol başarıyla kaydedilmiştir." };
        }

        public ICollection<RoleDto> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            var roleDtos = roles.Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return roleDtos;
        }

        public async Task<Result<string>> UpdateRoleAsync(string roleId, string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return new Result<string> { IsSuccess = false, ErrorMessage = "Rol adı boş olamaz." };
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return new Result<string> { IsSuccess = false, ErrorMessage = "Rol bulunamadı." };
            }

            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return new Result<string> { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }

            return new Result<string> { IsSuccess = true, Data = "Rol başarıyla güncellenmiştir." };
        }
    }
}