using EcommerceDev.Core.Entities;

namespace EcommerceDev.Core.Services;

public interface IJwtTokenService
{
    string GenerateToken(Customer customer);
    Guid? ValidateToken(string token);
}