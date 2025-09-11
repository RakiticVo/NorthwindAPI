using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Handler;

public static class AuthActionHandler
{
    public static CreateUserTokenRequest CreateToken(ITokenService tokenService, User user, string deviceType)
    {
        var accessToken = tokenService.CreateToken(user, deviceType, false);
        var refreshToken = tokenService.CreateToken(user, deviceType, true);
        var createUserTokenRequest = new CreateUserTokenRequest
        {
            UserId = user.Id,
            AccessToken = accessToken,
            TokenType = "bearer",
            DeviceType = deviceType,
            RefreshToken = refreshToken
        };
        return createUserTokenRequest;
    }
}