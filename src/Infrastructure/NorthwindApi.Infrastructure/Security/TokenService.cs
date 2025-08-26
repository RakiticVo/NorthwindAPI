using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace NorthwindApi.Infrastructure.Security;

public sealed class TokenService : ITokenService
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

    public string CreateToken(string userName, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userName),
            new(JwtRegisteredClaimNames.UniqueName, userName)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpMinutes),
            signingCredentials: _creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}