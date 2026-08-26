namespace RafeekRoomMate.Application.Features.RoomAppointments.Commands.ApproveRoomAppointment
{
    public sealed class ApproveRoomAppointmentCommand
    {
        public Guid RoomId { get; init; }

        public Guid AppointmentId { get; init; }
    }
}
