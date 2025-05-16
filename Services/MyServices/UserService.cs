using GameStore.Models.DTOs.User;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using GameStore.Services.IServices;

namespace GameStore.Services.MyServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                var userDtos = users.Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    RoleName = u.Role != null ? u.Role.RoleName : null,
                    CreatedAt = u.CreatedAt
                }).ToList();
                return userDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                RoleName = user.Role != null ? user.Role.RoleName : null,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDto> AddUserAsync(UserCreateDto createDto)
        {
            // Kiểm tra username và email trùng lặp
            if (await _userRepository.UsernameExistsAsync(createDto.Username))
            {
                throw new InvalidOperationException("Username is already taken.");
            }

            if (await _userRepository.EmailExistsAsync(createDto.Email))
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            // Tạo user mới
            var user = new User
            {
                Username = createDto.Username,
                Email = createDto.Email,
                FullName = createDto.FullName,
                RoleId = createDto.RoleId,
                CreatedAt = DateTime.Now
            };

            // Mã hóa mật khẩu
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createDto.Password);

            // Thêm user vào cơ sở dữ liệu
            await _userRepository.AddUserAsync(user);
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                RoleName = user.Role != null ? user.Role.RoleName : null,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDto> UpdateUserAsync(UserUpdateDto updateDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(updateDto.UserId);
            if (existingUser == null)
            {
                return null;
            }

            // Kiểm tra username và email trùng lặp
            if (await _userRepository.UsernameExistsAsync(updateDto.Username) &&
                existingUser.Username != updateDto.Username)
            {
                throw new InvalidOperationException("Username is already taken.");
            }

            if (await _userRepository.EmailExistsAsync(updateDto.Email) &&
                existingUser.Email != updateDto.Email)
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            // Cập nhật thông tin
            existingUser.Username = updateDto.Username;
            existingUser.Email = updateDto.Email;
            existingUser.FullName = updateDto.FullName;
            existingUser.RoleId = updateDto.RoleId;

            await _userRepository.UpdateUserAsync(existingUser);
            existingUser = await _userRepository.GetUserByIdAsync(existingUser.UserId);

            return new UserDto
            {
                UserId = existingUser.UserId,
                Username = existingUser.Username,
                Email = existingUser.Email,
                FullName = existingUser.FullName,
                RoleName = existingUser.Role != null ? existingUser.Role.RoleName : null,
                CreatedAt = existingUser.CreatedAt
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id);
                return true;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch
            {
                return false;
            }
        }
    }
}
