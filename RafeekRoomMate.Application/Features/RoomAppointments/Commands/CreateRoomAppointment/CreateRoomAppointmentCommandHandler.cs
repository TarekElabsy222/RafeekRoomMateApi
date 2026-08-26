using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Entities.Enums;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.RoomAppointments;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.RoomAppointments.Commands.CreateRoomAppointment
{
    public class CreateRoomAppointmentCommandHandler
    {
        #region fields
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<RoomAppointment> _appointmentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public CreateRoomAppointmentCommandHandler( IRepository<Room> roomRepository, IRepository<RoomAppointment> appointmentRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _appointmentRepository = appointmentRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions
        public async Task<Result<Guid>> HandleAsync(Guid roomId, CreateRoomAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<Guid>.Failure( "AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Get Room

            var roomSpecification = new RoomByIdSpecification(roomId);

            var room = await _roomRepository.GetBySpecificationAsync( roomSpecification, cancellationToken);

            if (room is null)
            {
                return Result<Guid>.Failure( "ROOM_NOT_FOUND", "Room was not found.");
            }

            // 3. User cannot request appointment
            // for his own room

            if (room.CreatedByUserId == userId.Value)
            {
                return Result<Guid>.Failure( "ROOM_OWNER_CANNOT_REQUEST", "The room owner cannot request an appointment for their own room.");
            }

            // 4. Room must be Available

            if (room.Status != RoomStatus.Available)
            {
                return Result<Guid>.Failure( "ROOM_NOT_AVAILABLE", "This room is not currently available.");
            }

            // 5. Check Existing Pending Appointment

            var pendingSpecification = new PendingRoomAppointmentSpecification( roomId, userId.Value);

            var existingAppointment = await _appointmentRepository .GetBySpecificationAsync( pendingSpecification, cancellationToken);

            if (existingAppointment is not null)
            {
                return Result<Guid>.Failure( "PENDING_APPOINTMENT_EXISTS", "You already have a pending appointment for this room.");
            }

            // 6. Create Appointment

            var appointment = new RoomAppointment( roomId, userId.Value, command.RequestedDate, command.RequestedStartTime, command.RequestedEndTime, command.Message);

            // 7. Add

            await _appointmentRepository.AddAsync( appointment, cancellationToken);

            // 8. Save

            await _unitOfWork.SaveChangesAsync( cancellationToken);

            // 9. Return Appointment Id

            return Result<Guid>.Success( appointment.Id);
        }
        #endregion
        }
}
