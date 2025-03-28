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
        private readonly IUserRoleService _userRoleService;
        private readonly IRedisCacheService _redisCacheService;
        public TokenService(IConfiguration configuration, IUserRoleService userRoleService, IRedisCacheService redisCacheService)
        {
            this.configuration = configuration;
            this._userRoleService = userRoleService;
            this._redisCacheService = redisCacheService;
        }

        public async Task<Result<GenerateTokenResponse>> GenerateTokenAsync(GenerateTokenRequest request)
        {
            try
            {
                SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AppSettings:Secret"]));

                // Claim'leri oluşturun
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, request.UserId),
                    new Claim(ClaimTypes.Name, request.UserName),
                };

                var roles = await _userRoleService.GetUserRolesAsync(new() { UserName = request.UserName });
                if (roles.IsSuccess)
                {
                    var redisKey = $"user:roles:{request.UserId}";
                    string roleDatas = string.Join(",", roles.Data);
                    
                    foreach (var role in roles.Data)
                    {
                        // claims.Add(new Claim(ClaimTypes.Role, role));
                        await _redisCacheService.SetAddAsync(redisKey, role);
                    }
                }

                var utcTime = DateTime.UtcNow;
                var tokenExpireDate = utcTime.Add(TimeSpan.FromMinutes(10));

                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                    issuer: configuration["AppSettings:ValidIssuer"],
                    audience: configuration["AppSettings:ValidAudience"],
                    claims: claims,
                    notBefore: utcTime,
                    expires: tokenExpireDate,
                    signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                );

                return new Result<GenerateTokenResponse>
                {
                    IsSuccess = true,
                    Data = new GenerateTokenResponse
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        TokenExpireDate = tokenExpireDate
                    }
                };
            }
            catch (Exception ex)
            {
                return new Result<GenerateTokenResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "Token oluşturulamadı." + ex.Message
                };
            }
        }
    }
}