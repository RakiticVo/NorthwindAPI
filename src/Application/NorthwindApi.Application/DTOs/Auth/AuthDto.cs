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
    public string RoleCode { get; init; } = "user";
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
    public int UserId { get; init; }
    public required string AccessToken { get; init; }
    public required string TokenType { get; init; }
    public required string DeviceType { get; init; }
    public required string RefreshToken { get; init; }
}

public record UpdateUserRequest(string? Email, string? RoleCode, string? NewPassword);

/// <summary>
/// Auth Response
/// </summary>
public record RegisterResponse(
    string Username,
    string Email,
    string UserRoleCode
);

public record AuthResponse(
    string UserName,
    string AccessToken,
    string RefreshToken
);
