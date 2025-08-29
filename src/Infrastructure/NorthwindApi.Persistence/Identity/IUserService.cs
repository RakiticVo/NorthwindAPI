using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Infrastructure.Identity;

public interface IUserService
{
    Task<User> RegisterAsync(string username, string email, string password, string? roleCode, CancellationToken cancellationToken);
    Task<User?> ValidateUserAsync(string username, string password, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetRolesAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task UpdateUserAsync(int id, string? email, string? roleCode, string? newPassword, CancellationToken cancellationToken);
    Task DeleteUserAsync(int id, CancellationToken cancellationToken);
}