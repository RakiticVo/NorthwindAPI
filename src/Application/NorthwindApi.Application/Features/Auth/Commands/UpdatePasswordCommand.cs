using System.Data;
using System.Windows.Input;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record UpdatePasswordCommand(int UserId, string UserPassword) : ICommand<ApiResponse>;

internal class UpdatePasswordCommandHandler(
    ICrudService<User, int> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdatePasswordCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdatePasswordCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var user = await crudService.GetByIdAsync(command.UserId);
            if (user == null) return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
            if (PasswordHasherHandler.Verify(command.UserPassword, user.HashedPassword))
                return new ApiResponse(StatusCodes.Status403Forbidden, "Duplicated Password!!!");
        
            // Hash password mới và update
            user.HashedPassword = PasswordHasherHandler.Hash(command.UserPassword);
            await crudService.UpdateAsync(user, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Password updated successfully");
        }
    }
}