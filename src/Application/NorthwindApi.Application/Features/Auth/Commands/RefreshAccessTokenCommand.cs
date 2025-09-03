using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.Features.Auth.Handler;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Infrastructure.Security;

namespace NorthwindApi.Application.Features.Auth.Commands;

public record RefreshAccessTokenCommand(RefreshTokenRequest RefreshTokenRequest) : ICommand<ApiResponse> { }

internal class RefreshAccessTokenCommandHandler(
    IRepository<User, int> userRepository,
    IRepository<UserToken, int> userTokenRepository,
    ICrudService<UserToken, int> crudService,
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IMapper mapper
) : ICommandHandler<RefreshAccessTokenCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(RefreshAccessTokenCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var user = await AuthActionHandler.CheckUserLoginHandler(userRepository, command.RefreshTokenRequest.Username);
            if (user is ApiResponse) return user;
            var existingUserToken = await userTokenRepository.FirstOrDefaultAsync(
                userTokenRepository.GetQueryableSet()
                    .Where(x => x.UserId == user.UserId && x.DeviceType == command.RefreshTokenRequest.DeviceType));
            if (existingUserToken is null) return new ApiResponse(StatusCodes.Status401Unauthorized, "Please Login!!!");
            
            var principal = tokenService.ValidateToken(existingUserToken.RefreshToken);
            if (principal == null) 
                return new ApiResponse(StatusCodes.Status401Unauthorized, "Token Invalid!!! Please login again!!!");
            
            var expiredToken = tokenService.IsTokenExpired(existingUserToken.RefreshToken);
            if (expiredToken)
                return new ApiResponse(StatusCodes.Status401Unauthorized, "Token Expired!!! Please login again!!!");
            
            var userTokenRequest = AuthActionHandler.CreateToken(tokenService, user, command.RefreshTokenRequest.DeviceType);
            var userToken = mapper.Map<UserToken>(userTokenRequest);
            existingUserToken.AccessToken = userToken.AccessToken;
            existingUserToken.RefreshToken = userToken.RefreshToken;
            existingUserToken.TokenType = userToken.TokenType;
            await crudService.UpdateAsync(existingUserToken, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var authResponse = new AuthResponse(user.Username, userTokenRequest.AccessToken, userToken.RefreshToken);
            return new ApiResponse(StatusCodes.Status200OK, "Refresh Token successfully!!!", authResponse);
        }
    }
}