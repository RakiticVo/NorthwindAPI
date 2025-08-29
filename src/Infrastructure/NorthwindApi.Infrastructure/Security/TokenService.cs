using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Infrastructure.Security;

public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;
    private readonly SigningCredentials _creds;

    public TokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
        _creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey)),
            SecurityAlgorithms.HmacSha256);
    }

    public string CreateToken(User user, bool isMobile, bool isRefreshToken)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var jti = Guid.NewGuid().ToString("N");
        var expireMinutes = isRefreshToken ? _settings.RefreshTokenExpireMinutes : _settings.TokenExpireMinutes;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Username),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Iat, now.ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Jti, jti),
        };
        if (!isRefreshToken)
        {
            claims.AddRange([
                new Claim("user_id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("user_role_code", user.UserRoleCode ?? "user"),
                new Claim("isMobile", isMobile ? "Mobile" : "Web")
            ]);
        }

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: _creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, GetTokenValidationParameters(), out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
    
    public bool IsTokenExpired(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token)) return true;
            
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            return true;
        }
    }
    
    private TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _settings.Issuer,
            ValidAudience = _settings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey)),
            ClockSkew = TimeSpan.Zero
        };
    }
}