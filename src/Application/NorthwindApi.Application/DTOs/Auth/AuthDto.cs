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
    [DefaultValue(2)] // Staff
    public int UserRoleId { get; init; } = 2;
}

public record LoginRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    [DefaultValue("web")]
    public string DeviceType { get; init; } = "web";
}

public record CreateUserTokenRequest
{
    public int UserId { get; init; }
    public required string AccessToken { get; init; }
    public required string TokenType { get; init; }
    public required string DeviceType { get; init; }
    public required string RefreshToken { get; init; }
}

public record UpdateUserPasswordRequest
{
    public string CurrentUserPassword { get; init; } = null!;
    public string NewUserPassword { get; init; } = null!;
    public string ConfirmUserPassword { get; init; } = null!;
}

/// <summary>
/// Auth Response
/// </summary>
public record RegisterResponse
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public int UserRoleId { get; init; }
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

public record UserResponse
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public int UserRoleId { get; init; }
    public string UserRoleName { get; init; } = null!;
}
