using Demo.Models;

namespace Demo.Interfaces
{
    public interface ITokenService
    {
        public Task<Result<GenerateTokenResponse>> GenerateTokenAsync(GenerateTokenRequest request);
    }
}