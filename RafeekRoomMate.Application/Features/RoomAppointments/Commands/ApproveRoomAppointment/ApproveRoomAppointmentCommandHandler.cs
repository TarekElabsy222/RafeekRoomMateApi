using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.RoomAppointments;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.RoomAppointments.Commands.ApproveRoomAppointment
{
    public class ApproveRoomAppointmentCommandHandler
    {
        #region fields

        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<RoomAppointment> _appointmentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region constructor

        public ApproveRoomAppointmentCommandHandler(
            IRepository<Room> roomRepository,
            IRepository<RoomAppointment> appointmentRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _appointmentRepository = appointmentRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region handle functions

        public async Task<Result<bool>> HandleAsync( ApproveRoomAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<bool>.Failure( "AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Get Room

            var roomSpecification = new RoomByIdSpecification(command.RoomId);

            var room = await _roomRepository.GetBySpecificationAsync( roomSpecification, cancellationToken);

            if (room is null)
            {
                return Result<bool>.Failure( "ROOM_NOT_FOUND", "Room was not found.");
            }

            // 3. Check Owner

            if (room.CreatedByUserId != userId.Value)
            {
                return Result<bool>.Failure( "ROOM_ACCESS_DENIED", "Only the room owner can approve appointments.");
            }

            // 4. Get Appointment

            var appointmentSpecification = new RoomAppointmentByIdSpecification( command.AppointmentId);

            var appointment = await _appointmentRepository.GetBySpecificationAsync( appointmentSpecification, cancellationToken);

            if (appointment is null)
            {
                return Result<bool>.Failure( "APPOINTMENT_NOT_FOUND", "Appointment was not found.");
            }

            // 5. Make sure Appointment belongs to Room

            if (appointment.RoomId != command.RoomId)
            {
                return Result<bool>.Failure( "APPOINTMENT_ROOM_MISMATCH", "Appointment does not belong to this room.");
            }

            // 6. Approve

            try
            {
                appointment.Approve();
            }
            catch (InvalidOperationException)
            {
                return Result<bool>.Failure( "APPOINTMENT_INVALID_STATUS", "Only pending appointments can be approved.");
            }

            // 7. Save

            await _unitOfWork.SaveChangesAsync( cancellationToken);

            // 8. Return

            return Result<bool>.Success(true);
        }

        #endregion
    }
}
