using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Handler;

public static class AuthActionHandler
{
    public static UserTokenRequest CreateToken(
        ITokenService tokenService,
        User user,
        string deviceType
    )
    {
        var accessToken = tokenService.CreateToken(user, deviceType, false);
        var refreshToken = tokenService.CreateToken(user, deviceType, true);
        var userTokenRequest = new UserTokenRequest
        {
            UserId = user.Id,
            AccessToken = accessToken,
            TokenType = "bearer",
            DeviceType = deviceType,
            RefreshToken = refreshToken
        };
        return userTokenRequest;
    }

    public static ApiResponse? CheckUserLoginHandler(
        User? user,
        string? password = null
    ) {
        if (user == null) return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
        if (password is null) return null;
        var isPasswordVerified = PasswordHasherHandler.Verify(password, user.HashedPassword);
        return !isPasswordVerified 
            ? new ApiResponse(StatusCodes.Status403Forbidden, "Incorrect password!!!") 
            : null;
    }
    
    public static ApiResponse? CheckUserTokenPrincipalAndExpiredHandler(UserToken? userToken, ITokenService tokenService) {
        if (userToken is null) return new ApiResponse(StatusCodes.Status401Unauthorized, "Please Login!!!");
            
        var principal = tokenService.ValidateToken(userToken.RefreshToken);
        if (principal == null) 
            return new ApiResponse(StatusCodes.Status401Unauthorized, "Token Invalid!!! Please login again!!!");
            
        var expiredToken = tokenService.IsTokenExpired(userToken.RefreshToken);
        return expiredToken 
            ? new ApiResponse(StatusCodes.Status401Unauthorized, "Token Expired!!! Please login again!!!") 
            : null;
    }
}