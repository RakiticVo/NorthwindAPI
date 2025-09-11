using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Order;
using NorthwindApi.Application.Features.Order.Commands;
using NorthwindApi.Application.Features.Order.Queries;

namespace NorthwindApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(Dispatcher dispatcher) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetOrders()
    {
        var data = await dispatcher.DispatchAsync(new GetOrders());
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get-by-page")]
    public async Task<IActionResult> GetOrdersByPage([FromQuery]GetOrderByPageRequest getOrderByPageRequest)
    {
        var data = await dispatcher.DispatchAsync(new GetOrdersByPage(getOrderByPageRequest));
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var data = await dispatcher.DispatchAsync(new GetOrderById(id));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPost("create-new")]
    public async Task<IActionResult> CreateOrder([FromBody]CreateOrderRequest request)
    {
        var data = await dispatcher.DispatchAsync(new CreateOrderCommand(request));
        return this.ReturnActionHandler(data);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateOrder([FromBody]UpdateOrderRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateOrderCommand(request));
        return this.ReturnActionHandler(data);
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var data = await dispatcher.DispatchAsync(new DeleteOrderCommand(id));
        return this.ReturnActionHandler(data);
    }
}
