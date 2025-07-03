using Microsoft.AspNetCore.Identity;
using WebApi.DTOs;

namespace WebApi.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}
