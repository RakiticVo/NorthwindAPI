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

public record RefreshAccessTokenCommand() : ICommand<ApiResponse> { }

internal class RefreshAccessTokenCommandHandler(
    ICrudService<User, int> crudServiceUser,
    ICrudService<UserToken, int> crudServiceUserToken,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IMapper mapper
) : ICommandHandler<RefreshAccessTokenCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(RefreshAccessTokenCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            try
            {
                var refreshToken = httpContextAccessor.HttpContext?.User?.FindFirst("refresh_token")?.Value;
            
                if (string.IsNullOrEmpty(refreshToken))
                    return new ApiResponse(StatusCodes.Status401Unauthorized, "User not authenticated or invalid token");
                
                // Validate và decode refresh token
                var principal = tokenService.ValidateToken(refreshToken);
            
                if (principal == null)
                {
                    return new ApiResponse(StatusCodes.Status401Unauthorized, "Token Invalid!!! Please login again!!!");
                }

                var expiredToken = tokenService.IsTokenExpired(refreshToken);
                if (expiredToken)
                    return new ApiResponse(StatusCodes.Status401Unauthorized, "Token Expired!!! Please login again!!!");
            
                // Lấy user_id từ refresh token claims
                var userIdClaim = principal.FindFirst("user_id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return new ApiResponse(StatusCodes.Status401Unauthorized, "Refresh Token Invalid!!! Please login again!!!");
                }
            
                // Kiểm tra user có tồn tại không
                var user = await crudServiceUser.GetByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse(StatusCodes.Status404NotFound, "User not found!!! Please login again!!!");
                }
            
                // Lấy user token theo user id và tạo token mới
                var userToken = await crudServiceUserToken.GetByIdAsync(userId);
                if (userToken == null)
                    return new ApiResponse(StatusCodes.Status404NotFound, "Token not found!!! Please login again!!!");
                var createTokenHandler = new CreateTokenHandler(tokenService, httpContextAccessor, user);
                var userTokenRequest = createTokenHandler.CreateToken();
                mapper.Map(userTokenRequest, userToken);
                await crudServiceUserToken.UpdateAsync(userToken, cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken); 
                var authResponse = new AuthResponse(user.Username, userToken.AccessToken, userToken.RefreshToken);
                return new ApiResponse(StatusCodes.Status200OK, "Login successfully", authResponse);
            }
            catch (SecurityTokenExpiredException)
            {
                return new ApiResponse(StatusCodes.Status401Unauthorized, "Token Expired!!! Please login again!!!");
            }
            catch (Exception)
            {
                return new ApiResponse(StatusCodes.Status403Forbidden, "Token Invalid!!! Please login again!!!");
            }
        }
    }
}