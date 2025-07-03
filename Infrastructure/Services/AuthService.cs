using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.DTOs;
using WebApi.Interfaces;


namespace Infrastructure.Services;

public class AuthService(
        UserManager<IdentityUser> userManager,
        IConfiguration config) : IAuthService
{
    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.Username);
        if (user == null) return null;

        var result = await userManager.CheckPasswordAsync(user, dto.Password);
        return !result
            ? null
            : GenerateJwtToken(user);
    }

    public Task<IdentityResult> RegisterAsync(RegisterDto dto)
    {
        throw new NotImplementedException();
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName!)
            };

        var secretKey = config["Jwt:Key"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}