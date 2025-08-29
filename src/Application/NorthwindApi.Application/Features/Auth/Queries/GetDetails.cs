using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Queries;

public record GetDetails : IQuery<ApiResponse> { }

internal class GetDetailsHandler(
    ICrudService<User, int> crudService,
    IHttpContextAccessor httpContextAccessor
) : IQueryHandler<GetDetails, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetDetails query, CancellationToken cancellationToken = default)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return new ApiResponse(StatusCodes.Status401Unauthorized, "User not authenticated or invalid token");
        }
        
        var user = await crudService.GetByIdAsync(userId);
        
        return user == null 
            ? new ApiResponse(StatusCodes.Status403Forbidden, "User not found")
            : new ApiResponse(StatusCodes.Status200OK, "Get User Successfully", user);
    }
}