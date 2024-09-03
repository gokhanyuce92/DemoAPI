using Demo.Entities;

namespace Demo.Repositories.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByUsernameAndPasswordAsync(string username, string password);
    }
}