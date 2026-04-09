using Microsoft.AspNetCore.Identity;

namespace EcommerceDev.Application.Commands.Auth.Register;

public class RegisterCommand
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Document { get; set; } = string.Empty;
}
