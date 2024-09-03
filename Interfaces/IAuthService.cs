using Demo.Models;

namespace Demo.Interfaces
{
    public interface IAuthService
    {
        public Task<Result<UserLoginResponse>> LoginUserAsync(UserLoginRequest request);
    }
}