namespace RafeekRoomMate.Application.Features.RoomAppointments.Commands.RejectRoomAppointment
{
    public sealed class RejectRoomAppointmentCommand
    {
        public Guid RoomId { get; init; }

        public Guid AppointmentId { get; init; }

        public string Reason { get; init; } = string.Empty;
    }
}
