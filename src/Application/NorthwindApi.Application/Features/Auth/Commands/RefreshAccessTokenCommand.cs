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

public record RefreshAccessTokenCommand : ICommand<ApiResponse>;

internal class RefreshAccessTokenCommandHandler(
    IRepository<User, int> userRepository,
    IRepository<UserToken, int> userTokenRepository,
    ICrudService<UserToken, int> crudService,
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
) : ICommandHandler<RefreshAccessTokenCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(RefreshAccessTokenCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var userName = httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? "system"; 
            var isMobile = httpContextAccessor.HttpContext?.User?.FindFirst("isMobile")?.Value ?? "Web"; 
            var user = await userRepository.FirstOrDefaultAsync(userRepository.GetQueryableSet().Where(x => x.Username == userName));
            var result = AuthActionHandler.CheckUserLoginHandler(user);
            if (result is not null) return result;
            
            var existingUserToken = await userTokenRepository.FirstOrDefaultAsync(
            userTokenRepository.GetQueryableSet()
                .Where(x => x.UserId == user!.Id && x.DeviceType.ToLower() == isMobile.ToLower()));
            var checkResult = AuthActionHandler.CheckUserTokenPrincipalAndExpiredHandler(existingUserToken, tokenService);
            if (checkResult is not null) return checkResult;
            
            var userTokenRequest = AuthActionHandler.CreateToken(tokenService, user!, isMobile);
            var userToken = mapper.Map<UserToken>(userTokenRequest);
            existingUserToken!.AccessToken = userToken.AccessToken;
            existingUserToken.RefreshToken = userToken.RefreshToken;
            existingUserToken.TokenType = userToken.TokenType;
            await crudService.UpdateAsync(existingUserToken, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var authResponse = new AuthResponse(user!.Username, userTokenRequest.AccessToken, userToken.RefreshToken);
            return new ApiResponse(StatusCodes.Status200OK, "Refresh Token successfully!!!", authResponse);
        }
    }
}