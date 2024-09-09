using Demo.DTOs.User;
using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.Services 
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<AddUserResponseDTO> AddAsync(AddUserRequestDTO addUserRequestDTO)
        {
            var identityUser = new AppUser
            {
                UserName = addUserRequestDTO.Username,
                Email = addUserRequestDTO.Email,
                FirstName = addUserRequestDTO.Username,
                LastName = addUserRequestDTO.Username
            };
            var passwordHash = _userManager.PasswordHasher;
            var hashedPassword = passwordHash.HashPassword(identityUser, addUserRequestDTO.Password);
            identityUser.PasswordHash = hashedPassword;

            var result = await _userManager.CreateAsync(identityUser);
            if (!result.Succeeded)
            {
                return new AddUserResponseDTO { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }

            return new AddUserResponseDTO { IsSuccess = true, Data = addUserRequestDTO };
        }
    }
}