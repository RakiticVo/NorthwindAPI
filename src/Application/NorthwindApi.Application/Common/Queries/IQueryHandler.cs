using NorthwindApi.Application.Common.Response;

namespace NorthwindApi.Application.Common.Queries;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<ApiResponse?> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
