namespace Domain.DTOs;

public class ResetPasswordDto
{
    public string UserName { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}