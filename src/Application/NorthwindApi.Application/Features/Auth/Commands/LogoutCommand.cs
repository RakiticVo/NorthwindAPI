using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record LogoutCommand : ICommand<ApiResponse>;

internal class LogoutAuthCommandHandler(
    ICrudService<User, int> userCrudService,
    ICrudService<UserToken, int> userTokenCrudService,
    IRepository<UserToken, int> userTokenRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : ICommandHandler<LogoutCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value ?? "0");
            var isMobile = httpContextAccessor.HttpContext?.User?.FindFirst("isMobile")?.Value ?? "Web"; 
            var user = await userCrudService.GetByIdAsync(userId);
            if (user == null) return new ApiResponse(StatusCodes.Status404NotFound, "User not found");
            
            var userToken = await userTokenRepository.FirstOrDefaultAsync(
            userTokenRepository.GetQueryableSet()
                .Where(x => x.UserId == userId &&
                    x.DeviceType.ToLower() == isMobile.ToLower()));
            if (userToken == null) return new ApiResponse(StatusCodes.Status403Forbidden, "User are not login");
            
            await userTokenCrudService.DeleteAsync(userToken, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "User successfully logged out");
        }
    }
}