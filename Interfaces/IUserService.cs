using Demo.DTOs.User;
using Demo.Models;

namespace Demo.Interfaces
{
    public interface IUserService
    {
        Task<AddUserResponseDTO> AddAsync(AddUserRequestDTO addUserRequestDTO);
    }
}