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
    public async Task<IActionResult> Register([FromBody]RegisterUserRequest registerUserRequest)
    {
        var data = await dispatcher.DispatchAsync(new RegisterUserCommand(registerUserRequest));
        return this.ReturnActionHandler(data);
    }
    
    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
    {
        var data = await dispatcher.DispatchAsync(new LoginCommand(loginRequest));
        return this.ReturnActionHandler(data);
    }
    
    // GET: api/Auth/user-details
    [HttpPost("user-details")]
    [Authorize]
    public async Task<IActionResult> UserDetails()
    {
        var data = await dispatcher.DispatchAsync(new GetUserDetailsById());
        return this.ReturnActionHandler(data);
    }
    
    // PUT: api/Auth/refresh-token
    [HttpPut("refresh-token")]
    [Authorize]
    public async Task<IActionResult> RefreshToken()
    {
        var data = await dispatcher.DispatchAsync(new RefreshAccessTokenCommand());
        return this.ReturnActionHandler(data);
    }
    
    // PUT: api/Auth/update-password
    [HttpPut("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody]string userPassword)
    {
        var data = await dispatcher.DispatchAsync(new UpdatePasswordCommand(userPassword));
        return this.ReturnActionHandler(data);
    }
    
    // POST: api/Auth/logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var data = await dispatcher.DispatchAsync(new LogoutCommand());
        return this.ReturnActionHandler(data);
    }
    
    // DELETE: api/Auth/delete
    [HttpDelete("delete/{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Delete(int id)
    {
        var data = await dispatcher.DispatchAsync(new DeleteUserCommand(id));
        return this.ReturnActionHandler(data);
    }
}
