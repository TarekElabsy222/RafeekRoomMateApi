using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.RoomFavorites;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.RoomFavorites.Commands.AddRoomFavorite
{
    public sealed class AddRoomFavoriteCommandHandler
    {
        #region fields
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<RoomFavorite> _favoriteRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public AddRoomFavoriteCommandHandler(
            IRepository<Room> roomRepository,
            IRepository<RoomFavorite> favoriteRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _favoriteRepository = favoriteRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions
        public async Task<Result<bool>> HandleAsync( Guid roomId, CancellationToken cancellationToken = default)
        {
            // 1. Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<bool>.Failure( "AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Check Room

            var roomSpecification = new RoomByIdSpecification(roomId);

            var room = await _roomRepository.GetBySpecificationAsync( roomSpecification, cancellationToken);

            if (room is null)
            {
                return Result<bool>.Failure( "ROOM_NOT_FOUND", "Room was not found.");
            }

            // 3. Check Existing Favorite

            var favoriteSpecification = new RoomFavoriteByUserAndRoomSpecification( userId.Value, roomId);

            var existingFavorite = await _favoriteRepository.GetBySpecificationAsync( favoriteSpecification, cancellationToken);

            if (existingFavorite is not null)
            {
                return Result<bool>.Failure( "ROOM_ALREADY_FAVORITED", "This room is already in your favorites.");
            }

            // 4. Create Favorite

            var favorite = new RoomFavorite( roomId, userId.Value);

            // 5. Add

            await _favoriteRepository.AddAsync( favorite, cancellationToken);

            // 6. Save

            await _unitOfWork.SaveChangesAsync( cancellationToken);

            // 7. Success

            return Result<bool>.Success(true);
        }
        #endregion
    }
}
