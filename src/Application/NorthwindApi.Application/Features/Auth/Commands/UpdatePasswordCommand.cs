using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record UpdatePasswordCommand(UpdateUserPasswordRequest UpdateUserPasswordRequest) : ICommand<ApiResponse>;

internal class UpdatePasswordCommandHandler(
    ICrudService<User, int> crudService,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : ICommandHandler<UpdatePasswordCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdatePasswordCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value ?? "0");
            if (userId == 0) return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
            var existingUser = await crudService.GetByIdAsync(userId);
            if (existingUser == null) return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
            
            if (!PasswordHasherHandler.Verify(command.UpdateUserPasswordRequest.CurrentUserPassword, existingUser.HashedPassword))
                return new ApiResponse(StatusCodes.Status403Forbidden, "Current Password not correct!!!");
            
            if(!command.UpdateUserPasswordRequest.NewUserPassword.Equals(command.UpdateUserPasswordRequest.ConfirmUserPassword))
                return new ApiResponse(StatusCodes.Status403Forbidden, "Password not confirmed!!!");
            
            if (PasswordHasherHandler.Verify(command.UpdateUserPasswordRequest.NewUserPassword, existingUser.HashedPassword))
                return new ApiResponse(StatusCodes.Status403Forbidden, "Duplicated Password!!!");
        
            // Hash password mới và update
            existingUser.HashedPassword = PasswordHasherHandler.Hash(command.UpdateUserPasswordRequest.NewUserPassword);
            await crudService.UpdateAsync(existingUser, token);
            return new ApiResponse(StatusCodes.Status200OK, "Password updated successfully!!!");
        }, cancellationToken: cancellationToken);
    }
}