using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.UpdateRoom
{
    public sealed class UpdateRoomCommandHandler
    {
        #region fields

        private readonly IRepository<Room> _roomRepository;

        private readonly IRepository<RoomMember> _roomMemberRepository;

        private readonly ICurrentUserService _currentUserService;

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region constructor

        public UpdateRoomCommandHandler(
            IRepository<Room> roomRepository,
            IRepository<RoomMember> roomMemberRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;

            _roomMemberRepository = roomMemberRepository;

            _currentUserService = currentUserService;

            _unitOfWork = unitOfWork;
        }

        #endregion

        #region handle functions

        public async Task<Result<bool>> HandleAsync(Guid roomId, UpdateRoomCommand command,CancellationToken cancellationToken = default)
        {
            // 1. Get Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<bool>.Failure("AUTH_UNAUTHORIZED","User is not authenticated.");
            }

            // 2. Get Room

            var specification = new RoomByIdSpecification(roomId);

            var room = await _roomRepository.GetBySpecificationAsync(specification,cancellationToken);

            if (room is null)
            {
                return Result<bool>.Failure("ROOM_NOT_FOUND", "Room was not found.");
            }

            // 3. Check Ownership

            if (room.CreatedByUserId != userId.Value)
            {
                return Result<bool>.Failure("ROOM_ACCESS_DENIED", "You are not allowed to modify this room.");
            }

            // 4. Check Current Occupants

            var membersSpecification = new ActiveRoomMembersByRoomSpecification(roomId);

            var currentRoommates = await _roomMemberRepository.CountAsync(membersSpecification,cancellationToken);

            if (command.MaximumOccupants < currentRoommates)
            {
                return Result<bool>.Failure("INVALID_MAXIMUM_OCCUPANTS", $"Maximum occupants cannot be less than the current number of occupants ({currentRoommates}).");
            }

            // 5. Update Room

            room.Update(
                command.Title.Trim(),
                command.Description.Trim(),
                command.City.Trim(),
                command.Area.Trim(),
                command.MonthlyRent,
                command.AvailableFrom,
                command.MaximumOccupants,
                command.RoomType,
                command.GenderPreference);

            // 6. Save Changes

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            // 7. Return Success

            return Result<bool>.Success(true);
        }

        #endregion
    }


}
