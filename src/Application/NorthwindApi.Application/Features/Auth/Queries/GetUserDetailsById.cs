using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Features.Auth.Queries;

public record GetUserDetailsById(int UserId) : IQuery<ApiResponse>;

internal class GetDetailsByIdHandler(ICrudService<User, int> crudService) : IQueryHandler<GetUserDetailsById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetUserDetailsById query, CancellationToken cancellationToken = default)
    {
        var user = await crudService.GetByIdAsync(query.UserId);
        return user == null 
            ? new ApiResponse(StatusCodes.Status404NotFound, "User not found")
            : new ApiResponse(StatusCodes.Status200OK, "Get User Successfully", user);
    }
}