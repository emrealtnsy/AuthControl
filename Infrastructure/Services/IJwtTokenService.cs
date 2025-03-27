using System.Security.Claims;
using AuthControl.Domain.Entities;

namespace AuthControl.Infrastructure.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user, IList<string> roles);
    ClaimsPrincipal ValidateToken(string token);
    
}