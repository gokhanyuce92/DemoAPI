using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService tokenService;
        private readonly UserManager<AppUser> _userManager;
        public AuthService(ITokenService tokenService, UserManager<AppUser> userManager)
        {
            this.tokenService = tokenService;
            this._userManager = userManager;
        }

            public async Task<Result<UserLoginResponse>> LoginUserAsync(UserLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
                return new Result<UserLoginResponse> { IsSuccess = false, ErrorMessage = "Kullanıcı adı boş olamaz." };
            if (string.IsNullOrEmpty(request.Password))
                return new Result<UserLoginResponse> { IsSuccess = false, ErrorMessage = "Şifre boş olamaz." };

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                // Kullanıcı adı ve şifre doğru
                var generateTokenResult = await tokenService.GenerateTokenAsync(new GenerateTokenRequest { UserId = user.Id.ToString(), UserName = user.UserName });
                if (!generateTokenResult.IsSuccess)
                {
                    return new Result<UserLoginResponse> { IsSuccess = false, ErrorMessage = generateTokenResult.ErrorMessage };
                }

                var response = new UserLoginResponse
                {
                    AccessTokenExpireDate = generateTokenResult.Data.TokenExpireDate,
                    AuthenticateResult = true,
                    AuthToken = generateTokenResult.Data.Token
                };

                return new Result<UserLoginResponse> { IsSuccess = true, Data = response };
            }
            else
            {
                return new Result<UserLoginResponse> { IsSuccess = false, ErrorMessage = "Geçersiz kullanıcı adı veya şifre." };
            }
        }
    }
}