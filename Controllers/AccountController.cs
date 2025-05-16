using GameStore.Models.DTOs.User;
using GameStore.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var user = await _authService.LoginAsync(loginDto);
                var token = _authService.GenerateJwtToken(user);

                return Ok(new
                {
                    Token = token,
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role?.RoleName,
                    CreatedAt = user.CreatedAt
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during login" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                var user = await _authService.RegisterAsync(registerDto);
                var token = _authService.GenerateJwtToken(user);

                return CreatedAtAction(nameof(Login), new { username = user.Username }, new
                {
                    Token = token,
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role?.RoleName,
                    CreatedAt = user.CreatedAt
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during registration" });
            }
        }
    }
}
