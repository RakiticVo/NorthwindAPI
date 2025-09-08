using System.ComponentModel;

namespace NorthwindApi.Application.DTOs.Auth;

/// <summary>
/// Auth Request
/// </summary>
public record RegisterUserRequest
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    [DefaultValue("user")]
    public string UserRoleCode { get; init; } = "user";
}

public record LoginRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    [DefaultValue("web")]
    public string DeviceType { get; init; } = "web";
}

public record UserTokenRequest
{
    public int Id { get; init; }
    public required string AccessToken { get; init; }
    public required string TokenType { get; init; }
    public required string DeviceType { get; init; }
    public required string RefreshToken { get; init; }
}

/// <summary>
/// Auth Response
/// </summary>
public record RegisterResponse
{
    public int Id { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string UserRoleCode { get; init; } = null!;
}

public record AuthResponse
{
    public string Username { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;

    public AuthResponse(string username, string accessToken, string refreshToken)
    {
        Username = username;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
};
