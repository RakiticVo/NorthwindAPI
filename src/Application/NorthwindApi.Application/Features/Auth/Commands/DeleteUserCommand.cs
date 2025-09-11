using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record DeleteUserCommand(int UserId) : ICommand<ApiResponse>;

internal class DeleteUserCommandHandler(
    ICrudService<User, int> crudService,
    ICrudService<UserToken, int> userTokenCrudService,
    IRepository<UserToken, int> userTokenRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteUserCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var existingUser = await crudService.GetByIdAsync(command.UserId);
            if (existingUser == null) return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");

            await crudService.DeleteAsync(existingUser, token);
            var userTokens = await userTokenRepository
                .ToListAsync(userTokenRepository.GetQueryableSet()
                    .Where(x => x.UserId == command.UserId));
            if (userTokens.Count < 0) return new ApiResponse(StatusCodes.Status200OK, "User deleted successfully!!!");
            foreach (var userToken in userTokens)
            {
                await userTokenCrudService.DeleteAsync(userToken, token);
            }
            return new ApiResponse(StatusCodes.Status200OK, "User deleted successfully!!!");
        }, cancellationToken: cancellationToken);
    }
}