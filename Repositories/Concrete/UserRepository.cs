using Demo.Entities;
using Demo.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repositories.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly MyDbContext _context;
        public UserRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> GetByUsernameAndPasswordAsync(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}