using System.Data;
using System.Windows.Input;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record UpdatePasswordCommand(string UserPassword) : ICommand<ApiResponse>;

internal class UpdatePasswordCommandHandler(
    ICrudService<User, int> crudService,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : ICommandHandler<UpdatePasswordCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdatePasswordCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value ?? "0");
            if (userId == 0) return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
            
            var existingUser = await crudService.GetByIdAsync(userId);
            if (existingUser == null) return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
            if (PasswordHasherHandler.Verify(command.UserPassword, existingUser.HashedPassword))
                return new ApiResponse(StatusCodes.Status403Forbidden, "Duplicated Password!!!");
        
            // Hash password mới và update
            existingUser.HashedPassword = PasswordHasherHandler.Hash(command.UserPassword);
            await crudService.UpdateAsync(existingUser, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Password updated successfully!!!");
        }
    }
}