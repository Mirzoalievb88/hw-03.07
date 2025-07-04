using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; } = null!;
    public string Email { get; set; } = null!;
}