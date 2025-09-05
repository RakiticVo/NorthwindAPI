using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.Features.Auth.Commands;
using NorthwindApi.Application.Features.Auth.Queries;

namespace NorthwindApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(Dispatcher dispatcher) : ControllerBase
{
    // POST: api/Auth/register
    [HttpPost("register")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var data = await dispatcher.DispatchAsync(new RegisterUserCommand(registerRequest));
        return this.ReturnActionHandler(data);
    }
    
    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var data = await dispatcher.DispatchAsync(new LoginCommand(loginRequest));
        return this.ReturnActionHandler(data);
    }
    
    // GET: api/Auth/user-details
    [HttpPost("user-details")]
    [Authorize]
    public async Task<IActionResult> UserDetails(int userId)
    {
        var data = await dispatcher.DispatchAsync(new GetUserDetailsById(userId));
        return this.ReturnActionHandler(data);
    }
    
    // PUT: api/Auth/refresh-token
    [HttpPut("refresh-token")]
    [Authorize]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var data = await dispatcher.DispatchAsync(new RefreshAccessTokenCommand(refreshTokenRequest));
        return this.ReturnActionHandler(data);
    }
    
    // PUT: api/Auth/update-password
    [HttpPut("update-password")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdatePassword(int userId, string userPassword)
    {
        var data = await dispatcher.DispatchAsync(new UpdatePasswordCommand(userId, userPassword));
        return this.ReturnActionHandler(data);
    }
    
    // POST: api/Auth/logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(int userId, string deviceType)
    {
        var data = await dispatcher.DispatchAsync(new LogoutCommand(userId, deviceType));
        return this.ReturnActionHandler(data);
    }
    
    // DELETE: api/Auth/delete
    [HttpDelete("delete")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Delete(int userId)
    {
        var data = await dispatcher.DispatchAsync(new DeleteUserCommand(userId));
        return this.ReturnActionHandler(data);
    }
}
