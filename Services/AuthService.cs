using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Services
{
    public class AuthService : IAuthService
    {
        private readonly MyDbContext _context;
        private readonly ITokenService tokenService;
        private readonly UserManager<AppUser> _userManager;
        public AuthService(ITokenService tokenService, UserManager<AppUser> userManager, MyDbContext context)
        {
            this.tokenService = tokenService;
            this._userManager = userManager;
            this._context = context;
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
                var generateTokenResult = await tokenService.GenerateTokenAsync(new GenerateTokenRequest { Username = user.UserName });
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
                var existingToken = await _context.UserTokens.FirstOrDefaultAsync(
                    ut => ut.UserId == user.Id && 
                        ut.LoginProvider == "DataProtectorTokenProvider<AppUser>" && 
                        ut.Name == "Token");
                if (existingToken != null)
                {
                    existingToken.Value = response.AuthToken;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await _context.UserTokens.AddAsync(new IdentityUserToken<string>
                    {
                        UserId = user.Id,
                        LoginProvider = "DataProtectorTokenProvider<AppUser>",
                        Name = "Token",
                        Value = response.AuthToken
                    });
                 
                    await _context.SaveChangesAsync();
                }

                return new Result<UserLoginResponse> { IsSuccess = true, Data = response };
            }
            else
            {
                return new Result<UserLoginResponse> { IsSuccess = false, ErrorMessage = "Geçersiz kullanıcı adı veya şifre." };
            }
        }
    }
}