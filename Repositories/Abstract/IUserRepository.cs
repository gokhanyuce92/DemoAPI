using Demo.Entities;

namespace Demo.Repositories.Abstract
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<AppUser> GetByUsernameAsync(string username);
        Task<AppUser> GetByUsernameAndPasswordAsync(string username, string password);
    }
}