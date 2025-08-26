namespace NorthwindApi.Infrastructure.Security;

public interface ITokenService
{
    string CreateToken(string userName, IEnumerable<string> roles);
}