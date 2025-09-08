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

public record LoginCommand(LoginRequest LoginRequest) : ICommand<ApiResponse>;

internal class LoginAuthCommandHandler(
    IRepository<User, int> userRepository,
    IRepository<UserToken, int> userTokenRepository,
    ITokenService tokenService,
    ICrudService<UserToken, int> crudService,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<LoginCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var user = await userRepository.FirstOrDefaultAsync(userRepository.GetQueryableSet()
                .Where(x => x.Username == command.LoginRequest.Username));
            var result = AuthActionHandler.CheckUserLoginHandler(
                user,
                command.LoginRequest.Password
            );
            if (result is not null) return result;
            
            var userTokenRequest = AuthActionHandler.CreateToken(tokenService, user!, command.LoginRequest.DeviceType.ToLower());
            var userToken = mapper.Map<UserToken>(userTokenRequest);
            var existingUserToken = await userTokenRepository.FirstOrDefaultAsync(
            userTokenRepository.GetQueryableSet()
                .Where(x => x.UserId == user!.Id 
                    && x.DeviceType.ToLower() == command.LoginRequest.DeviceType.ToLower()));
            if (existingUserToken == null) await crudService.AddAsync(userToken, cancellationToken);
            else 
            {
                existingUserToken.AccessToken = userToken.AccessToken;
                existingUserToken.RefreshToken = userToken.RefreshToken;
                existingUserToken.TokenType = userToken.TokenType;
                await crudService.UpdateAsync(existingUserToken, cancellationToken);
            }

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var authResponse = new AuthResponse(user!.Username, userTokenRequest.AccessToken, userToken.RefreshToken);
            return new ApiResponse(StatusCodes.Status200OK, "Login successfully", authResponse);
        }
    }
    
    
}