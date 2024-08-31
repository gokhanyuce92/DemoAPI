using Demo.Interfaces;
using Demo.Models;

namespace Demo.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService tokenService;
        public AuthService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            UserLoginResponse response = new();

            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Username == "admin" && request.Password == "123456")
            {
                var generateToken = await tokenService.GenerateTokenAsync(new GenerateTokenRequest { Username = request.Username });

                response.AccessTokenExpireDate = generateToken.TokenExpireDate;
                response.AuthenticateResult = true;
                response.AuthToken = generateToken.Token;
            }

            return response;
        }
    }
}