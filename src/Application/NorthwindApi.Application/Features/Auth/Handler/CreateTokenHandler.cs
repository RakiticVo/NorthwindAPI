using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Handler;

public class CreateTokenHandler(
    ITokenService tokenService,
    IHttpContextAccessor httpContextAccessor,
    User user
)
{
    public UserTokenRequest CreateToken()
    {
        var isMobile = IsMobileDevice();
        var accessToken = tokenService.CreateToken(user, isMobile, false);
        var refreshToken = tokenService.CreateToken(user, isMobile, true);
        var userTokenRequest = new UserTokenRequest
        {
            UserId = user.Id,
            AccessToken = accessToken,
            TokenType = "bearer",
            DeviceType = "deviceType",
            RefreshToken = refreshToken
        };
        return userTokenRequest;
    }
    private bool IsMobileDevice()
    {
        var userAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "";
    
        var mobileKeywords = new[] 
        {
            "Mobile", "Android", "iPhone", "iPad", "Windows Phone", 
            "BlackBerry", "Opera Mini", "IEMobile"
        };
    
        return mobileKeywords.Any(keyword => 
            userAgent.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
}