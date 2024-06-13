using CollegeApp.Data.Repository;
using CollegeApp.Data;
using AutoMapper;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using CollegeApp.Models;

namespace CollegeApp.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<User> _userRepository;
        public UserService(ICollegeRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateUserAsync(UserDTO dto)
        {
            //Old way
            //if(dto == null)
            //    throw new ArgumentNullException(nameof(dto));

            //New way
            ArgumentNullException.ThrowIfNull(dto, $"the argument {nameof(dto)} is null");

            var existingUser = await _userRepository.GetAsync(u => u.Username.Equals(dto.Username));

            if (existingUser != null)
            {
                throw new Exception("The username already taken");
            }

            User user = _mapper.Map<User>(dto);
            user.IsDeleted = false;
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(dto.Password);
                user.Password = passwordHash.PasswordHash;
                user.PasswordSalt = passwordHash.Salt;
            }

            await _userRepository.CreateAsync(user);

            return true;
        }
        public (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password)
        {
            //Create the salt
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //Create Password Hash
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return (hash, Convert.ToBase64String(salt));
        }
    }
}
