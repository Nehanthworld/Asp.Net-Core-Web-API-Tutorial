using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserDTO dto);
        (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password);
    }
}
