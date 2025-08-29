using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.Features.Auth.Handler;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record LoginCommand(LoginRequest LoginRequest) : ICommand<ApiResponse>
{
    public LoginRequest LoginRequest { get; init; } = LoginRequest;
}

internal class LoginAuthCommandHandler(
    IRepository<User, int> repository,
    ITokenService tokenService,
    ICrudService<UserToken, int> crudService,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
) : ICommandHandler<LoginCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var user = await repository.FirstOrDefaultAsync(repository.GetQueryableSet()
                .Where(x => x.Username == command.LoginRequest.Username));
            if (user == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!!");
            var isPasswordVerified = PasswordHasher.Verify(command.LoginRequest.Password, user.HashedPassword);
            if (!isPasswordVerified)
                return new ApiResponse(StatusCodes.Status403Forbidden, "Incorrect password!!!");

            var createTokenHandler = new CreateTokenHandler(tokenService, httpContextAccessor, user);
            var userTokenRequest = createTokenHandler.CreateToken();
            var userToken = mapper.Map<UserToken>(userTokenRequest);
            await crudService.AddAsync(userToken, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var authResponse = new AuthResponse(user.Username, userTokenRequest.AccessToken, userToken.RefreshToken);
            return new ApiResponse(StatusCodes.Status200OK, "Login successfully", authResponse);
        }
    }
    
    
}