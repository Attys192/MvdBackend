using Microsoft.AspNetCore.Mvc;
using MvdBackend.Models;
using MvdBackend.Repositories;
using MvdBackend.DTOs;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
namespace MvdBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost("employee-login")]
        public async Task<IActionResult> EmployeeLogin([FromBody] LoginDto dto)
        {
            try
            {
                _logger.LogInformation($"Login attempt for user: {dto.Username}");

                var user = await _userRepository.GetByUsernameAsync(dto.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                {
                    _logger.LogWarning($"Invalid login attempt for user: {dto.Username}");
                    return Unauthorized("Неверный логин или пароль");
                }

                _logger.LogInformation($"User {dto.Username} logged in successfully");

                return Ok(new
                {
                    user.Id,
                    user.Username,
                    Role = user.Role.Name,
                    Employee = new
                    {
                        user.Employee.Id,
                        FullName = $"{user.Employee.LastName} {user.Employee.FirstName} {user.Employee.Patronymic}"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, "Ошибка при авторизации");
            }
        }
    }
}   
