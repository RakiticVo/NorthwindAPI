using System.Security.Claims;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Infrastructure.Security;

public interface ITokenService
{
    string CreateToken(User user, bool isMobile, bool isRefreshToken);
    ClaimsPrincipal? ValidateToken(string token);
    bool IsTokenExpired(string token);
}