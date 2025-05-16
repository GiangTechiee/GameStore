using GameStore.Models.DTOs.User;
using GameStore.Models.Entities;

namespace GameStore.Services.IService
{
    public interface IAuthService
    {
        Task<User> LoginAsync(UserLoginDto loginDto);
        Task<User> RegisterAsync(UserRegisterDto registerDto);

        string GenerateJwtToken(User user);
    }
}
