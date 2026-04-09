namespace EcommerceDev.Application.Commands.Auth.Login
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
