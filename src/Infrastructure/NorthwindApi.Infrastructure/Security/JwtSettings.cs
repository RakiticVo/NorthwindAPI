namespace NorthwindApi.Infrastructure.Security;

public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int TokenExpireMinutes { get; set; }
    public int RefreshTokenExpireMinutes { get; set; }
}