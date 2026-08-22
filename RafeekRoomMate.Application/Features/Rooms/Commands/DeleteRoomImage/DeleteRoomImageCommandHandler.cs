using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.RoomImages;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.DeleteRoomImage
{
    public class DeleteRoomImageCommandHandler
    {
        #region fields

        private readonly IRepository<Room> _roomRepository;

        private readonly IRepository<RoomImage> _roomImageRepository;

        private readonly ICurrentUserService _currentUserService;

        private readonly IFileStorageService _fileStorageService;

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region constructor

        public DeleteRoomImageCommandHandler(
            IRepository<Room> roomRepository,
            IRepository<RoomImage> roomImageRepository,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _roomImageRepository = roomImageRepository;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region handle functions

        public async Task<Result<bool>> HandleAsync( Guid roomId, Guid imageId, CancellationToken cancellationToken = default)
        {
            // 1. Get Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<bool>.Failure( "AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Get Room

            var roomSpecification = new RoomByIdSpecification(roomId);

            var room = await _roomRepository.GetBySpecificationAsync( roomSpecification, cancellationToken);

            if (room is null)
            {
                return Result<bool>.Failure( "ROOM_NOT_FOUND", "Room was not found.");
            }

            // 3. Check Ownership

            if (room.CreatedByUserId != userId.Value)
            {
                return Result<bool>.Failure( "ROOM_ACCESS_DENIED", "You are not allowed to modify this room.");
            }

            // 4. Get Image

            var imageSpecification = new RoomImageByIdAndRoomSpecification( roomId, imageId);

            var image = await _roomImageRepository.GetBySpecificationAsync( imageSpecification, cancellationToken);

            if (image is null)
            {
                return Result<bool>.Failure( "ROOM_IMAGE_NOT_FOUND", "Room image was not found.");
            }

            // 5. Delete Physical File

            if (!string.IsNullOrWhiteSpace(image.ImageKey))
            {
                await _fileStorageService.DeleteAsync( image.ImageKey, cancellationToken);
            }

            // 6. Delete Database Record

             await _roomImageRepository.DeleteAsync(image);

            // 7. Save Changes

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            // 8. Return Success 

            return Result<bool>.Success(true); 
        }

        #endregion
    }
}

