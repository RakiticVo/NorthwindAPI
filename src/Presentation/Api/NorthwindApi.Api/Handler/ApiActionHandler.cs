using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Response;

namespace NorthwindApi.Api.Handler;

public static class ApiActionHandler
{
    public static IActionResult ReturnActionHandler(this ControllerBase controller, ApiResponse data)
    {
        return data.StatusCode switch
        {
            StatusCodes.Status200OK => controller.Ok(data),
            StatusCodes.Status201Created => controller.StatusCode(StatusCodes.Status201Created, data),
            StatusCodes.Status401Unauthorized => controller.Unauthorized(data),
            StatusCodes.Status403Forbidden => controller.StatusCode(StatusCodes.Status403Forbidden, data),
            _ => controller.NotFound(data)
        };
    }
}