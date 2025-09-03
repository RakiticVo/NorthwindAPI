namespace NorthwindApi.Application.DTOs.Auth;

/// <summary>
/// Auth Request
/// </summary>
public record RegisterRequest(string Username, string Email, string Password, string? RoleCode = "user");

public record LoginRequest(string Username, string Password, string DeviceType);
public record RefreshTokenRequest(string Username, string DeviceType);

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
