using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Queries;

public record GetDetailsById(int UserId) : IQuery<ApiResponse>
{
    public int UserId { get; set; } = UserId;
}

internal class GetDetailsByIdHandler(
    ICrudService<User, int> crudService
) : IQueryHandler<GetDetailsById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetDetailsById query, CancellationToken cancellationToken = default)
    {
        var user = await crudService.GetByIdAsync(query.UserId);
        return user == null 
            ? new ApiResponse(StatusCodes.Status403Forbidden, "User not found")
            : new ApiResponse(StatusCodes.Status200OK, "Get User Successfully", user);
    }
}