using NorthwindApi.Domain.Entities;
using NorthwindApi.Persistence;

namespace NorthwindApi.Infrastructure.Identity;

public class UserService(NorthwindContext db) : IUserService
{
    public Task<User> RegisterAsync(string username, string email, string password, string? roleCode,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User?> ValidateUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(int id, string? email, string? roleCode, string? newPassword, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}