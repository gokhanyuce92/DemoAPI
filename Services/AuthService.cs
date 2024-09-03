using Demo.Interfaces;
using Demo.Models;

namespace Demo.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService tokenService;
        private readonly IUserService userService;
        public AuthService(ITokenService tokenService, IUserService userService)
        {
            this.tokenService = tokenService;
            this.userService = userService;
        }

        public async Task<Result<UserLoginResponse>> LoginUserAsync(UserLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
                return new Result<UserLoginResponse> { IsSuccess = false, ErrorMessage = "Kullanıcı adı boş olamaz." };
            if (string.IsNullOrEmpty(request.Password))
                return new Result<UserLoginResponse> { IsSuccess = false, ErrorMessage = "Parola boş olamaz." };

            var user = await userService.GetByUsernameAndPasswordAsync(request.Username, request.Password);
            if (user == null)
                return new Result<UserLoginResponse> { IsSuccess = false, ErrorMessage = "Geçersiz kullanıcı adı veya parola." };

            var generateTokenResult = await tokenService.GenerateTokenAsync(new GenerateTokenRequest { Username = user.Username });
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
    }
}