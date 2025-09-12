using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Persistence.Identity;

public class AdminInitializerService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var crudService = scope.ServiceProvider.GetRequiredService<ICrudService<User, int>>();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<User, int>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        // Kiểm tra số lượng user
        var userCount = await repository.GetQueryableSet().CountAsync(cancellationToken);
        if (userCount > 0) return;

        // Tạo user admin nếu chưa có user nào
        var adminRequest = new RegisterUserRequest
        {
            Username = "admin",
            Email = "admin@northwind.com",
            Password = "admin@123123",
            UserRoleId = 1
        };

        await unitOfWork.ExecuteInTransactionAsync<ApiResponse>(async token =>
        {
            var registerRequest = adminRequest with
            {
                Password = PasswordHasherHandler.Hash(adminRequest.Password)
            };

            var user = mapper.Map<User>(registerRequest);
            await crudService.AddAsync(user, token);
            var newUser = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.Username == user.Username));
            var registerResponse = mapper.Map<RegisterResponse>(newUser);
            return new ApiResponse(StatusCodes.Status201Created, "User created successfully!!!", registerResponse);
        }, cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}