using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Validator.Auth;

public class AuthValidation
{
    public static async Task<ApiResponse?> UserRegisterValidate(
        RegisterUserRequest registerUserRequest,
        ICrudService<User, int> crudService
    )
    {
        var userList = await crudService.GetAsync();
        foreach (var u in userList)
        {
            if (u.Username == registerUserRequest.Username)
            {
                return new ApiResponse(StatusCodes.Status409Conflict, "User already exists");
            }

            if (u.Email == registerUserRequest.Email)
            {
                return new ApiResponse(StatusCodes.Status409Conflict, "Email already exists");
            }
        }

        return null;
    }
    public static ApiResponse? UserLoginValidate(User? user, string? password = null) {
        if (user == null) return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
        if (password is null) return null;
        var isPasswordVerified = PasswordHasherHandler.Verify(password, user.HashedPassword);
        return !isPasswordVerified 
            ? new ApiResponse(StatusCodes.Status403Forbidden, "Incorrect password!!!") 
            : null;
    }
    
    public static ApiResponse? UserTokenPrincipalAndExpiredValidate(UserToken? userToken, ITokenService tokenService) {
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