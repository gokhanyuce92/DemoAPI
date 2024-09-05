using Demo.Entities;
using Demo.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repositories.Concrete
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        private readonly MyDbContext _context;
        public UserRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AppUser> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetByUsernameAndPasswordAsync(string username, string password)
        {
            // return await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }
    }
}