using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.Features.Auth.Handler;
using NorthwindApi.Application.Validator.Auth;
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
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var userName = httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? "system"; 
            var isMobile = httpContextAccessor.HttpContext?.User?.FindFirst("isMobile")?.Value ?? "Web"; 
            var existingUser = await userRepository.FirstOrDefaultAsync(
                userRepository.GetQueryableSet()
                    .Where(x => x.Username == userName));
            var result = AuthValidation.UserLoginValidate(existingUser);
            if (result is not null) return result;
            
            var existingUserToken = await userTokenRepository.FirstOrDefaultAsync(
                userTokenRepository.GetQueryableSet()
                    .Where(x => x.UserId == existingUser!.Id && 
                            x.DeviceType.ToLower() == isMobile.ToLower()));
            var checkResult = AuthValidation.UserTokenPrincipalAndExpiredValidate(existingUserToken, tokenService);
            if (checkResult is not null) return checkResult;
            
            var userTokenRequest = AuthActionHandler.CreateToken(tokenService, existingUser!, isMobile);
            var userToken = mapper.Map<UserToken>(userTokenRequest);
            existingUserToken!.AccessToken = userToken.AccessToken;
            existingUserToken.RefreshToken = userToken.RefreshToken;
            existingUserToken.TokenType = userToken.TokenType;
            await crudService.UpdateAsync(existingUserToken, token);
            var authResponse = new AuthResponse(existingUser!.Username, userTokenRequest.AccessToken, userToken.RefreshToken);
            return new ApiResponse(StatusCodes.Status200OK, "Token refreshed successfully!!!", authResponse);
        }, cancellationToken: cancellationToken);
    }
}