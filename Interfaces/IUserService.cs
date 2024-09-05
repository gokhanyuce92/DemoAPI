using Demo.Entities;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IUserService
    {
        Task<Result<string>> AddAsync(UserLoginRequest user);
    }
}