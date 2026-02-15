using Microsoft.AspNetCore.Mvc;
using EasyIce.WebApi.Models;

namespace EasyIce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // В реальном приложении: проверка пароля, генерация JWT
        if (request.Email == "test@test.com" && request.Password == "password")
        {
            return Ok(new { Token = "mock-jwt-token-12345", UserId = Guid.NewGuid(), SkillLevel = "Novice" });
        }
        
        return Unauthorized();
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        // В реальном приложении: создание пользователя
        return Ok(new { Token = "mock-jwt-token-new-user", UserId = Guid.NewGuid(), SkillLevel = request.SkillLevel });
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string DisplayName { get; set; }
    public string SkillLevel { get; set; }
}
