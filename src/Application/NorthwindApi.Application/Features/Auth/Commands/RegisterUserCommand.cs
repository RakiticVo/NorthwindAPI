using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.Validator;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record RegisterUserCommand(RegisterUserRequest RegisterUserRequest) : ICommand<ApiResponse>;

internal class RegisterAuthCommandHandler(
    ICrudService<User, int> crudService,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<RegisterUserCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(RegisterUserCommand userCommand, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var registerRequest = userCommand.RegisterUserRequest with { Password = PasswordHasherHandler.Hash(userCommand.RegisterUserRequest.Password) };
            var checkValidate = await SharedValidation.RegisterUserValidation(userCommand.RegisterUserRequest, crudService);
            if (checkValidate != null) return checkValidate;
            
            var user = mapper.Map<User>(registerRequest);
            await crudService.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var registerResponse = mapper.Map<RegisterResponse>(user);
            return new ApiResponse(StatusCodes.Status201Created, "Create user successfully", registerResponse);
        }
    }
}