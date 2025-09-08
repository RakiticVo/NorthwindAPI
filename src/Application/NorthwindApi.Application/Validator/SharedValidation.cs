using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Validator;

public class SharedValidation()
{
    public static async Task<ApiResponse?> RegisterUserValidation(
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
}