using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Commands;

public class LogoutCommand : ICommand<ApiResponse> { }

internal class LogoutAuthCommandHandler(
    ICrudService<User, int> crudService,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
) : ICommandHandler<LogoutCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return new ApiResponse(StatusCodes.Status401Unauthorized, "User not authenticated or invalid token");
            }
            var user = await crudService.GetByIdAsync(userId);
            if (user == null)
                return new ApiResponse(StatusCodes.Status403Forbidden, "User not found");
            await crudService.DeleteAsync(user, cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "User successfully logged out");
        }
    }
}