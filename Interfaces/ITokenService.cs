using Demo.Models;

namespace Demo.Interfaces
{
    public interface ITokenService
    {
        public Task<GenerateTokenResponse> GenerateTokenAsync(GenerateTokenRequest request);
    }
}