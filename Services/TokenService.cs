using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task<Result<GenerateTokenResponse>> GenerateTokenAsync(GenerateTokenRequest request)
        {
            try
            {
                SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AppSettings:Secret"]));

                var dateTimeNow = DateTime.UtcNow;

                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                    issuer: configuration["AppSettings:ValidIssuer"],
                    audience: configuration["AppSettings:ValidAudience"],
                    claims: new List<Claim> {
                    new Claim("Username", request.Username)
                    },
                    notBefore: dateTimeNow,
                    expires: dateTimeNow.Add(TimeSpan.FromMinutes(500)),
                    signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                );

                return Task.FromResult(new Result<GenerateTokenResponse>
                {
                    IsSuccess = true,
                    Data = new GenerateTokenResponse
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        TokenExpireDate = dateTimeNow.Add(TimeSpan.FromMinutes(500))
                    }
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Result<GenerateTokenResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "Token oluşturulamadı." + ex.Message
                });
            }
        }
    }
}