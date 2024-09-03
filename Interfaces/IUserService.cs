using Demo.Entities;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByUsernameAndPasswordAsync(string username, string password);
        Task<Result<User>> AddAsync(User user);
    }
}