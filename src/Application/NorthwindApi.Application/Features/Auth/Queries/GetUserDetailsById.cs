using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Queries;

public record GetUserDetailsById : IQuery<ApiResponse>;

internal class GetDetailsByIdHandler(
    ICrudService<User, int> crudService,
    IHttpContextAccessor httpContextAccessor
) : IQueryHandler<GetUserDetailsById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetUserDetailsById query, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value ?? "0");
        var user = await crudService.GetByIdAsync(userId);
        return user == null 
            ? new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get User Successfully!!!", user);
    }
}