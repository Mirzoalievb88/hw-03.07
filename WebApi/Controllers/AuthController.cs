using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Interfaces;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await service.RegisterAsync(dto);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok("User registered successfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await service.LoginAsync(dto);
            if (token == null) return Unauthorized();
            return Ok(new { token });
        }
    }
} 