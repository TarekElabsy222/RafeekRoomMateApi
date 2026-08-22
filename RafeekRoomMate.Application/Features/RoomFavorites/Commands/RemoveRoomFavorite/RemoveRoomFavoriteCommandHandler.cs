using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.RoomFavorites;

namespace RafeekRoomMate.Application.Features.RoomFavorites.Commands.RemoveRoomFavorite
{
    public sealed class RemoveRoomFavoriteCommandHandler
    {
        #region fields

        private readonly IRepository<RoomFavorite> _favoriteRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor

        public RemoveRoomFavoriteCommandHandler(
            IRepository<RoomFavorite> favoriteRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _favoriteRepository = favoriteRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region handle

        public async Task<Result<bool>> HandleAsync(Guid roomId, CancellationToken cancellationToken = default)
        {
            // 1. Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<bool>.Failure( "AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Get Favorite

            var specification =
                new RoomFavoriteByUserAndRoomSpecification( userId.Value, roomId);

            var favorite = await _favoriteRepository.GetBySpecificationAsync( specification, cancellationToken);

            if (favorite is null)
            {
                return Result<bool>.Failure( "ROOM_FAVORITE_NOT_FOUND", "This room is not in your favorites.");
            }

            // 3. Delete Favorite

            await _favoriteRepository.DeleteAsync(favorite);

            // 4. Save

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Success

            return Result<bool>.Success(true);
        }

        #endregion
    }
}

