using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Rooms.DTOs.CreateRoom;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.CreateRoom
{
    public class CreateRoomCommandHandler
    {
        #region fields
        private readonly IRepository<Room> _roomRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public CreateRoomCommandHandler(IRepository<Room> roomRepository,ICurrentUserService currentUserService,IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions
        public async Task<Result<RoomDto>> HandleAsync(CreateRoomCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Get authenticated user
            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<RoomDto>.Failure("AUTH_UNAUTHORIZED","User is not authenticated.");
            }
            // 2. Create Room entity
            var room = new Room(userId.Value,command.Title,command.Description,
                                command.City,command.RoomType,command.GenderPreference,
                                command.Area,command.MonthlyRent,command.AvailableFrom,command.MaximumOccupants);

            // 3. Add Room using Generic Repository
            await _roomRepository.AddAsync(room,cancellationToken);

            // 4. Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            // 5. Map to DTO
            var response = new RoomDto
            {
                Id = room.Id,
                CreatedByUserId = room.CreatedByUserId,

                Title = room.Title,
                Description = room.Description,

                City = room.City,
                Area = room.Area,

                MonthlyRent = room.MonthlyRent,
                AvailableFrom = room.AvailableFrom,

                MaximumOccupants = room.MaximumOccupants,

                RoomType = room.RoomType,
                GenderPreference = room.GenderPreference,

                Status = room.Status,

                CreatedAt = room.CreatedAt,
                UpdatedAt = room.UpdatedAt
            };

            return Result<RoomDto>.Success(response);
        }

        #endregion
    }
}
