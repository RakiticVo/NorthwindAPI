using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.Features.Auth.Commands;

namespace NorthwindApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(Dispatcher dispatcher) : ControllerBase
{
    // POST: api/Auth/register
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse>> Register(RegisterRequest registerRequest)
    {
        var data = await dispatcher.DispatchAsync(new RegisterUserCommand(registerRequest));
        if (data.StatusCode == StatusCodes.Status201Created)
            return Ok(data);
        return BadRequest(data);
    }
    
    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse>> Login(LoginRequest loginRequest)
    {
        var data = await dispatcher.DispatchAsync(new LoginCommand(loginRequest));
        return data.StatusCode switch
        {
            StatusCodes.Status200OK => Ok(data),
            StatusCodes.Status401Unauthorized => Unauthorized(data),
            _ => NotFound(data)
        };
    }
}
