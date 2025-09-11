using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.Validator;
using NorthwindApi.Application.Validator.Auth;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record RegisterUserCommand(RegisterUserRequest RegisterUserRequest) : ICommand<ApiResponse>;

internal class RegisterAuthCommandHandler(
    ICrudService<User, int> crudService,
    IRepository<User, int> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<RegisterUserCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(RegisterUserCommand userCommand, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var registerRequest = userCommand.RegisterUserRequest with
            {
                Password = PasswordHasherHandler.Hash(userCommand.RegisterUserRequest.Password)
            };
            var checkValidate =
                await AuthValidation.UserRegisterValidate(userCommand.RegisterUserRequest, crudService);
            if (checkValidate != null) return checkValidate;

            var user = mapper.Map<User>(registerRequest);
            await crudService.AddAsync(user, token);
            var newUser = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.Username == user.Username));
            var registerResponse = mapper.Map<RegisterResponse>(newUser);
            return new ApiResponse(StatusCodes.Status201Created, "User created successfully!!!", registerResponse);
        }, cancellationToken: cancellationToken);
    }
}