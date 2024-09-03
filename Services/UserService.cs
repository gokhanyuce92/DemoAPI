using Demo.Entities;
using Demo.Interfaces;
using Demo.Models;
using Demo.Repositories.Abstract;

namespace Demo.Services {
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<User>> AddAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> GetByUsernameAndPasswordAsync(string username, string password)
        {
            return await _userRepository.GetByUsernameAndPasswordAsync(username, password);
        }
    }
}