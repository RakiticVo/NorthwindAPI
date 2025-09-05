using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record LogoutCommand(int UserId, string DeviceType) : ICommand<ApiResponse>;

internal class LogoutAuthCommandHandler(
    ICrudService<User, int> userCrudService,
    ICrudService<UserToken, int> userTokenCrudService,
    IRepository<UserToken, int> userTokenRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<LogoutCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var user = await userCrudService.GetByIdAsync(command.UserId);
            if (user == null)
                return new ApiResponse(StatusCodes.Status403Forbidden, "User not found");
            var userToken = await userTokenRepository.FirstOrDefaultAsync(
            userTokenRepository.GetQueryableSet()
                .Where(x => 
                    x.DeviceType.ToLower() == command.DeviceType.ToLower() &&
                    x.UserId == command.UserId));
            if (userToken == null)
                return new ApiResponse(StatusCodes.Status403Forbidden, "User are not login");
            await userTokenCrudService.DeleteAsync(userToken, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "User successfully logged out");
        }
    }
}