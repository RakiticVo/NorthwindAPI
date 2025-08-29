using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record DeleteUserCommand(int UserId) : ICommand<ApiResponse>
{
    public int UserId { get; init; } = UserId;
}

internal class DeleteUserCommandHandler(
    ICrudService<User, int> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteUserCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var user = await crudService.GetByIdAsync(command.UserId);
            if (user == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");

            await crudService.DeleteAsync(user, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Delete User successfully");
        }
    }
}