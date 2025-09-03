using Microsoft.AspNetCore.Identity;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Infrastructure.Security;

public static class PasswordHasherHandler
{
    private static readonly PasswordHasher<User> Hasher = new();

    public static string Hash(string password)
    {
        return Hasher.HashPassword(null!, password);
    }

    public static bool Verify(string password, string storedHash)
    {
        var result = Hasher.VerifyHashedPassword(null!, storedHash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}