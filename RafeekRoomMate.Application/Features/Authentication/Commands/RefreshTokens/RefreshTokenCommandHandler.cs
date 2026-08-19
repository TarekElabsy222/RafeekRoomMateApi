using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Models;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Authentication.Common;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.RefreshTokens;
using RafeekRoomMate.Domain.Specifications.UserProfiles;
namespace RafeekRoomMate.Application.Features.Authentication.Commands.RefreshTokens
{
    public class RefreshTokenCommandHandler
    {
        #region fields
        private readonly IIdentityService _identityService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IDateTimeProvider _dateTimeProvider;
        #endregion

        #region constructor
        public RefreshTokenCommandHandler(IIdentityService identityService, IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider, IUnitOfWork unitOfWork, IRepository<RefreshToken> refreshTokenRepository, IRepository<UserProfile> userProfileRepository)
        {
            _identityService = identityService;
            _jwtProvider = jwtProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _userProfileRepository = userProfileRepository;
        }
        #endregion

        #region handle functions
        public async Task<Result<AuthResponse>> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            // 1. Find the refresh token using Specification

            var specification = new RefreshTokenByTokenSpecification(command.RefreshToken);

            var refreshToken = await _refreshTokenRepository.GetBySpecificationAsync(specification, cancellationToken);

            // 2. Check if the refresh token exists
            if (refreshToken is null)
            {
                return Result<AuthResponse>.Failure("AUTH_INVALID_REFRESH_TOKEN","Invalid refresh token.");
            }
            // 3. Check if the refresh token is active
            if (!refreshToken.IsActive)
            {
                return Result<AuthResponse>.Failure("AUTH_INVALID_REFRESH_TOKEN","Refresh token is expired or revoked.");
            }
            // 4. Get the user using UserId
            var user = await _identityService.GetUserAsync(refreshToken.UserId,cancellationToken);
            if (user is null)
            {
                return Result<AuthResponse>.Failure("AUTH_USER_NOT_FOUND","Unable to load user.");
            }
            var profileSpecification = new UserProfileByUserIdSpecification(refreshToken.UserId);

            var profile =  await _userProfileRepository.GetBySpecificationAsync( profileSpecification, cancellationToken);

            var isProfileCompleted = profile is not null;
            // 5. Get user roles
            var roles = await _identityService.GetUserRolesAsync(refreshToken.UserId, cancellationToken);

            // 6. Prepare token user
            var tokenUser = new TokenUser
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles
            };

            // 7. Generate a new Access Token
            var accessToken = _jwtProvider.GenerateAccessToken(tokenUser);
            var accessTokenExpiresAt = _jwtProvider.GetAccessTokenExpiration();

            // 8. Generate a new Refresh Token
            var newRefreshToken = _refreshTokenProvider.GenerateRefreshToken();

            var newRefreshTokenExpiresAt = _refreshTokenProvider.GetRefreshTokenExpiration();

            // 9. Revoke the old Refresh Token
            refreshToken.RevokedAt = DateTime.UtcNow;

            // 10. Store the replacement token
            refreshToken.ReplacedByToken = newRefreshToken;

            // 10. Create the new RefreshToken entity
            var newRefreshTokenEntity = new RefreshToken(refreshToken.UserId,newRefreshToken,newRefreshTokenExpiresAt);
            
            // 11. Add the new Refresh Token
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity,cancellationToken);

            // 12. Commit changes
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            // 13. Build AuthResponse
            // =========================================================

            var response = new AuthResponse
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiresAt = newRefreshTokenExpiresAt,
                User = user,
                IsProfileCompleted = isProfileCompleted
            };

            return Result<AuthResponse>.Success(response);
        }
        #endregion
    }
}
