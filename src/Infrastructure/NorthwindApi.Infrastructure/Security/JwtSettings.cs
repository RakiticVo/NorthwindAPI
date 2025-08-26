namespace NorthwindApi.Infrastructure.Security;

public class JwtSettings
{
    public string Issuer { get; set; } = "northwind.api";
    public string Audience { get; set; } = "northwind.api.clients";
    public string SigningKey { get; set; } = null!;
    public int ExpMinutes { get; set; } = 120;
}