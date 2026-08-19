using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Models;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Authentication.Common;
using RafeekRoomMate.Domain.Constant;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;

namespace RafeekRoomMate.Application.Features.Authentication.Register
{
    public class RegisterCommandHandler
    {
        #region fields
        private readonly IIdentityService _identityService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public RegisterCommandHandler(IIdentityService identityService, IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider, IRepository<RefreshToken> refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _jwtProvider = jwtProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions
        public async Task<Result<AuthResponse>> HandleAsync( RegisterCommand command, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.BeginTransactionAsync( cancellationToken);
            try
            {
                // 1. Check if email already exists
                var emailExists = await _identityService.EmailExistsAsync(command.Email, cancellationToken);
                if (emailExists)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<AuthResponse>.Failure("AUTH_EMAIL_EXISTS", "Email is already registered.");
                }

                // 2. Create user
                var createResult = await _identityService.CreateUserAsync(
                        command.FirstName, command.LastName,
                        command.Email, command.Password, cancellationToken);

                if (!createResult.Succeeded || createResult.UserId is null)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<AuthResponse>.Failure("AUTH_USER_CREATION_FAILED", string.Join(", ", createResult.Errors));
                }

                var userId = createResult.UserId.Value;

                // 3. Add default role
                var roleAdded = await _identityService.AddToRoleAsync(userId, Roles.User, cancellationToken);

                if (!roleAdded)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<AuthResponse>.Failure("AUTH_ROLE_ASSIGNMENT_FAILED", "Could not assign the default user role.");
                }

                // 4. Get created user
                var user = await _identityService.GetUserAsync(userId, cancellationToken);

                if (user is null)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<AuthResponse>.Failure("AUTH_USER_NOT_FOUND", "User could not be loaded.");
                }

                // 5. Get user roles
                var roles = await _identityService.GetUserRolesAsync(userId, cancellationToken);

                // 6. Prepare token user
                var tokenUser = new TokenUser
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = roles
                };

                // 7. Generate access token
                var accessToken = _jwtProvider.GenerateAccessToken(tokenUser);

                var accessTokenExpiresAt = _jwtProvider.GetAccessTokenExpiration();

                // 8. Generate refresh token
                var refreshToken = _refreshTokenProvider.GenerateRefreshToken();

                var refreshTokenExpiresAt = _refreshTokenProvider.GetRefreshTokenExpiration();

                // 9. Create refresh token entity
                var refreshTokenEntity = new RefreshToken(userId, refreshToken, refreshTokenExpiresAt);

                // 10. Add refresh token using Generic Repository
                await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

                // 11. Commit everything
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                // 12. Prepare response
                var response = new AuthResponse
                {
                    AccessToken = accessToken,
                    AccessTokenExpiresAt = accessTokenExpiresAt,

                    RefreshToken = refreshToken,
                    RefreshTokenExpiresAt = refreshTokenExpiresAt,

                    User = user,

                    IsProfileCompleted = false
                };

                return Result<AuthResponse>.Success(response);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(
                    cancellationToken);

                throw;
            }
        }
        #endregion

    }
}
