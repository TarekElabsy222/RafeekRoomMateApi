using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.DeleteRoom
{
    public sealed class DeleteRoomCommandHandler
    {
        #region fields

        private readonly IRepository<Room> _roomRepository;

        private readonly IRepository<RoomMember> _roomMemberRepository;
        private readonly IRepository<RoomImage> _roomImageRepository;

        private readonly ICurrentUserService _currentUserService;

        private readonly IFileStorageService _fileStorageService;

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region constructor

        public DeleteRoomCommandHandler(
            IRepository<Room> roomRepository,
            IRepository<RoomMember> roomMemberRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService,
            IRepository<RoomImage> roomImageRepository)
        {
            _roomRepository = roomRepository;
            _roomMemberRepository = roomMemberRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _roomImageRepository = roomImageRepository;
        }

        #endregion

        #region handle functions

        public async Task<Result<bool>> HandleAsync( Guid roomId, CancellationToken cancellationToken = default)
        {
            // 1. Get Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<bool>.Failure( "AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Get Room

            var specification = new RoomByIdSpecification(roomId);

            var room =
                await _roomRepository.GetBySpecificationAsync( specification, cancellationToken);

            if (room is null)
            {
                return Result<bool>.Failure( "ROOM_NOT_FOUND", "Room was not found.");
            }

            // 3. Check Ownership

            if (room.CreatedByUserId != userId.Value)
            {
                return Result<bool>.Failure( "ROOM_ACCESS_DENIED", "You are not allowed to delete this room.");
            }

            // 4. Check Active Roommates

            var membersSpecification = new ActiveRoomMembersByRoomSpecification(roomId);

            var currentRoommates = await _roomMemberRepository.CountAsync( membersSpecification, cancellationToken);

            if (currentRoommates > 0)
            {
                return Result<bool>.Failure( "ROOM_HAS_ACTIVE_MEMBERS", "The room cannot be deleted while it has active roommates.");
            }

            // 5. Get Room Images

            var imagesSpecification = new RoomImagesByRoomSpecification(roomId);

            var roomImages = await _roomImageRepository.ListAsync(imagesSpecification, cancellationToken);

            // 6. Delete Physical Files

            foreach (var image in roomImages)
            {
                if (!string.IsNullOrWhiteSpace(image.ImageKey))
                {
                    await _fileStorageService.DeleteAsync( image.ImageKey, cancellationToken);
                }
            }
            // 7. Delete Room Images From Database

            foreach (var image in roomImages)
            {
                await _roomImageRepository.DeleteAsync(image);
            }

            // 8. Delete Room

            await _roomRepository.DeleteAsync(room,cancellationToken);

            // 9. Save Changes

            await _unitOfWork.SaveChangesAsync( cancellationToken);

            // 10. Return Success

            return Result<bool>.Success(true);
        }

        #endregion
    }

}
