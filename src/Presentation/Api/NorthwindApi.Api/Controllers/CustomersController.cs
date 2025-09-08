using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Customer;
using NorthwindApi.Application.Features.Customer.Commands;
using NorthwindApi.Application.Features.Customer.Queries;

namespace NorthwindApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController(Dispatcher dispatcher) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetCustomers()
    {
        var data = await dispatcher.DispatchAsync(new GetCustomers());
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetCustomer(string id)
    {
        var data = await dispatcher.DispatchAsync(new GetCustomerById(id));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPost("create-new")]
    public async Task<IActionResult> CreateCustomer([FromBody]CreateCustomerRequest request)
    {
        var data = await dispatcher.DispatchAsync(new CreateCustomerCommand(request));
        return this.ReturnActionHandler(data);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateCustomer([FromBody]UpdateCustomerRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateCustomerCommand(request));
        return this.ReturnActionHandler(data);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        var data = await dispatcher.DispatchAsync(new DeleteCustomerCommand(id));
        return this.ReturnActionHandler(data);
    }
}
