using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.Services {
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<Result<string>> AddAsync(UserLoginRequest user)
        {
            var identityUser = new AppUser
            {
                UserName = user.Username,
                Email = user.Email,
                FirstName = user.Username,
                LastName = user.Username
            };
            var passwordHash = _userManager.PasswordHasher;
            var hashedPassword = passwordHash.HashPassword(identityUser, user.Password);
            identityUser.PasswordHash = hashedPassword;

            var result = await _userManager.CreateAsync(identityUser);
            if (!result.Succeeded)
            {
                return new Result<string> { IsSuccess = false, ErrorMessage = result.Errors.FirstOrDefault().Description };
            }

            return new Result<string> { IsSuccess = true, Data = "Kullanıcı başarıyla kaydedilmiştir." };
        }
    }
}