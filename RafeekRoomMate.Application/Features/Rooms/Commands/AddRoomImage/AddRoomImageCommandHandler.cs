using Microsoft.Extensions.Options;
using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Options;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.AddRoomImage
{
    public class AddRoomImageCommandHandler
    {
        #region fields
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<RoomImage> _roomImageRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IRoomImageValidator _roomImageValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoomImageOptions _options;
        #endregion

        #region constructor
        public AddRoomImageCommandHandler(
        IRepository<Room> roomRepository,
        IRepository<RoomImage> roomImageRepository,
        ICurrentUserService currentUserService,
        IFileStorageService fileStorageService,
        IRoomImageValidator roomImageValidator,
        IUnitOfWork unitOfWork,
        IOptions<RoomImageOptions> options)
        {
            _roomRepository = roomRepository;
            _roomImageRepository = roomImageRepository;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _roomImageValidator = roomImageValidator;
            _unitOfWork = unitOfWork;
            _options = options.Value;
        }
        #endregion

        #region handle functions
        public async Task<Result<Guid>> HandleAsync(AddRoomImageCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Get Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<Guid>.Failure("AUTH_UNAUTHORIZED","User is not authenticated.");
            }

            // 2. Validate basic command data

            if (command.Stream is null || command.Length <= 0)
            {
                return Result<Guid>.Failure( "INVALID_ROOM_IMAGE", "Room image is required.");
            }

            // 3. Get Room using Specification

            var specification =new RoomByIdSpecification(command.RoomId);

            var room = await _roomRepository.GetBySpecificationAsync(specification,cancellationToken);
            if (room is null)
            {
                return Result<Guid>.Failure("ROOM_NOT_FOUND", "Room was not found.");
            }
            // 4. Make sure Current User owns the Room

            if (room.CreatedByUserId != userId.Value)
            {
                return Result<Guid>.Failure("ROOM_ACCESS_DENIED","You are not allowed to modify this room.");
            }

            // 5. Validate Image

            var isValid =await _roomImageValidator.IsValidAsync(command.Stream,
                    command.FileName,command.ContentType,command.Length,cancellationToken);

            if (!isValid)
            {
                return Result<Guid>.Failure("INVALID_ROOM_IMAGE","Invalid room image.");
            }


            // 6. Check Maximum Room Images
            var imageSpecification =new RoomImagesByRoomSpecification(command.RoomId);
            var imageCount = await _roomImageRepository.CountAsync(imageSpecification, cancellationToken);

            if (imageCount >= _options.MaxImagesPerRoom)
            {
                return Result<Guid>.Failure("ROOM_IMAGE_LIMIT_REACHED",$"A room can have a maximum of {_options.MaxImagesPerRoom} images.");
            }

            var displayOrder = imageCount + 1;

            // 7. Upload File

            var folder =$"rooms/{command.RoomId}";

            var imageKey =await _fileStorageService.UploadAsync(command.Stream,command.FileName,
                    command.ContentType, folder, cancellationToken);

            // 8. Create RoomImage Entity

            var roomImage =new RoomImage(command.RoomId,imageKey, displayOrder);

            // 9. Add RoomImage using Generic Repository

            await _roomImageRepository.AddAsync(roomImage,cancellationToken);

            // 10. Save Database Changes

            await _unitOfWork.SaveChangesAsync(cancellationToken);


            // 11. Return Created Image Id

            return Result<Guid>.Success(roomImage.Id);

        }

        #endregion
        }
}
