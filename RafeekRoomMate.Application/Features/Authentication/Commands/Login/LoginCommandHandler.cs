using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Models;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Authentication.Common;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.UserProfiles;

namespace RafeekRoomMate.Application.Features.Authentication.Commands.Login
{
    public sealed class LoginCommandHandler
    {
        #region fields
        private readonly IIdentityService _identityService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenProvider _refreshTokenPrivider;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public LoginCommandHandler(IIdentityService identityService,
            IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider,
            IRepository<RefreshToken> refreshTokenRepository, IUnitOfWork unitOfWork, IRepository<UserProfile> userProfileRepository)
        {
            _identityService = identityService;
            _jwtProvider = jwtProvider;
            _refreshTokenPrivider = refreshTokenProvider;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _userProfileRepository = userProfileRepository;
        }
        #endregion

        #region handle functions
        public async Task<Result<AuthResponse>> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            // 1. Find user ID by email

            var userId = await _identityService.GetUserIdByEmailAsync(command.Email,cancellationToken);
                       
            if (userId is null)
            {
                return Result<AuthResponse>.Failure("AUTH_INVALID_CREDENTIALS", "Invalid email or password.");
            }

            // 2. Check password

            var passwordValid = await _identityService.CheckPasswordAsync(userId.Value,command.Password,cancellationToken);

            if (!passwordValid)
            {
                return Result<AuthResponse>.Failure("AUTH_INVALID_CREDENTIALS","Invalid email or password.");
            }

            // 3. Get user 

            var user = await _identityService.GetUserAsync( userId.Value,  cancellationToken);

            if (user is null)
            {
                return Result<AuthResponse>.Failure("AUTH_USER_NOT_FOUND","Unable to load user.");
            }
            var profileSpecification =  new UserProfileByUserIdSpecification(user.Id);

            var profile = await _userProfileRepository.GetBySpecificationAsync( profileSpecification, cancellationToken);

            var isProfileCompleted = profile is not null;

            // 4. Get user roles
            var roles = await _identityService.GetUserRolesAsync(userId.Value,cancellationToken);

            // 5. Prepare token user
            var tokenUser = new TokenUser
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles
            };

            // 6. Generate Access Token

            var accessToken = _jwtProvider.GenerateAccessToken(tokenUser);
            var accessTokenExpiresAt = _jwtProvider.GetAccessTokenExpiration();

            // 7. Generate Refresh Token

            var refreshToken = _refreshTokenPrivider.GenerateRefreshToken();

            var refreshTokenExpiresAt = _refreshTokenPrivider.GetRefreshTokenExpiration();

            // 8. Create refresh token entity
            var refreshTokenEntity = new RefreshToken(userId.Value, refreshToken, refreshTokenExpiresAt);

            // 9. Add refresh token 
            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

            // 10. Save everything and commit transaction

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // 11. Build authentication response

            var response = new AuthResponse
            {
                AccessToken = accessToken,

                AccessTokenExpiresAt =accessTokenExpiresAt,

                RefreshToken = refreshToken,

                RefreshTokenExpiresAt = refreshTokenExpiresAt,

                User = user,

                IsProfileCompleted = isProfileCompleted
            };
            // 12. Return successful result

            return Result<AuthResponse>.Success(response);
        }
    }
        #endregion
}

