using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserDTO dto);
        Task<List<UserReadonlyDTO>> GetUsersAsync();
        Task<UserReadonlyDTO> GetUserByIdAsync(int id);
        Task<UserReadonlyDTO> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserAsync(UserDTO dto);
        Task<bool> DeleteUser(int userId);
        (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password);
    }
}
