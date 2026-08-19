using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.RefreshTokens;

namespace RafeekRoomMate.Application.Features.Authentication.Commands.RevokeRefreshToken
{
    public class RevokeRefreshTokenCommandHandler
    {
        #region fields
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public RevokeRefreshTokenCommandHandler(IRepository<RefreshToken> refreshTokenRepository,IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions
        public async Task<bool> HandleAsync(RevokeRefreshTokenCommand command,CancellationToken cancellationToken = default)
        {
            // 1. Find refresh token
            var specification = new RefreshTokenByTokenSpecification(command.RefreshToken);
            var refreshToken = await _refreshTokenRepository.GetBySpecificationAsync(specification,cancellationToken);
          
            // 2. Token doesn't exist
            if (refreshToken is null)  return false;

            // 3. Already revoked
            if (refreshToken.IsRevoked) return true;

            // 4. Revoke token
            refreshToken.RevokedAt = DateTime.UtcNow;

            // 5. Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
        #endregion
        }
}
