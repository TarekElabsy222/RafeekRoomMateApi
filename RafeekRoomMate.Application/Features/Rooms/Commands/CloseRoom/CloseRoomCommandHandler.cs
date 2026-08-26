using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.CloseRoom
{
    public class CloseRoomCommandHandler
    {
        #region fields
        private readonly IRepository<Room> _roomRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public CloseRoomCommandHandler( IRepository<Room> roomRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions
        public async Task<Result<bool>> HandleAsync( Guid roomId, CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<bool>.Failure( "AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Get Room

            var specification = new RoomByIdSpecification(roomId);

            var room = await _roomRepository.GetBySpecificationAsync( specification, cancellationToken);

            if (room is null)
            {
                return Result<bool>.Failure( "ROOM_NOT_FOUND", "Room was not found.");
            }

            // 3. Check Creator
            if (room.CreatedByUserId != userId.Value)
            {
                return Result<bool>.Failure( "ROOM_ACCESS_DENIED", "You are not allowed to modify this room.");
            }

            // 4. Close Room

            room.Close();

            // 5. Save

            await _unitOfWork.SaveChangesAsync( cancellationToken);

            return Result<bool>.Success(true);
        }


        #endregion
    }

}
