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

    public static async Task<dynamic> CheckUserLoginHandler(
        IRepository<User, int> userRepository,
        string userName,
        string? password = null
    )
    {
        var user = await userRepository.FirstOrDefaultAsync(userRepository.GetQueryableSet()
            .Where(x => x.Username == userName));
        if (user == null)
            return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
        if (password != null)
        {
            var isPasswordVerified = PasswordHasherHandler.Verify(password, user.HashedPassword);
            if (!isPasswordVerified)
                return new ApiResponse(StatusCodes.Status403Forbidden, "Incorrect password!!!");   
        }
        return user;
    }
}