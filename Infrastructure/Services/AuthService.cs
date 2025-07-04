using Domain.ApiResponse;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Interfaces;


namespace Infrastructure.Services;

public class AuthService(
        UserManager<IdentityUser> userManager,
        IHttpContextAccessor contextAccessor,
        IConfiguration config) : IAuthService
{
    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.Username);
        if (user == null) return null;

        var result = await userManager.CheckPasswordAsync(user, dto.Password);
        return !result
            ? null
            : await GenerateJwtToken(user);
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
    {
        var user = new IdentityUser { UserName = dto.Username };
        var result = await userManager.CreateAsync(user, dto.Password);
        return result;
    }


    public async Task<Response<string>> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var userName = contextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var user = await userManager.FindByNameAsync(userName!);
        var email = dto.Email;
        var newPassword = new Random().Next(10000, 99999).ToString();
        if (user == null)
        {
            return new Response<string>("User not Found", HttpStatusCode.NotFound);
        }

        var result = new Response<string>(default!, "All Worked");
        if (result.IsSuccess == true)
        {
            var fromEmail = "bahodurmirzoaliev2008@gmail.com";
            var toEmail = $"{email}";
            var password = "hqxv lscf gllk awcf";
            var subject = "Password";
            var body = $"You changed you password you new Password is {newPassword}";

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            var mail = new MailMessage(fromEmail, toEmail, subject, body);

            smtp.Send(mail);
        }
        return new Response<string>(default!, "All Worked");
    }


    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Name, user.UserName!)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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

    public async Task<Response<string>> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.UserName);
        if (user == null)
        {
            return new Response<string>("User NOt Found", HttpStatusCode.NotFound);
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        await userManager.ResetPasswordAsync(user, token, dto.NewPassword);
        var result = new Response<string>(default!, "All Worked");
        if (result.IsSuccess == true)
        {
            var fromEmail = "bahodurmirzoaliev2008@gmail.com";
            var toEmail = "bahodurmirzoaliev2008@gmail.com";
            var password = "hqxv lscf gllk awcf";
            var subject = "Password";
            var body = $"You Password been changed your new password is {dto.NewPassword}";

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            var mail = new MailMessage(fromEmail, toEmail, subject, body);

            smtp.Send(mail);
        }
        return new Response<string>(default!, "All Worked");
    }
}