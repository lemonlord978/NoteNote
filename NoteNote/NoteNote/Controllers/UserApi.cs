using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteNote.Dtos;
using NoteNote.Services.IServices;

namespace NoteNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApi : ControllerBase
    {
        private readonly IUserService _userService;
        public UserApi(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.LoginAsync(loginDto.Username, loginDto.Password);

                return Ok(new { message = "Login successful", userId = user.UserId });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        
        [HttpPost("getUserById")]
        public async Task<IActionResult> GetUserById([FromBody] int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);

                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Register the user using the repository
                var user = await _userService.RegisterAsync(registerDto.Username, registerDto.Password, registerDto.Email);

                return Ok(new { message = "User registered successfully", userId = user.UserId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
