using GameStore.Models.DTOs.User;
using GameStore.Models.Entities;

namespace GameStore.Services.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> AddUserAsync(UserCreateDto createDto);
        Task<UserDto> UpdateUserAsync(UserUpdateDto updateDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
