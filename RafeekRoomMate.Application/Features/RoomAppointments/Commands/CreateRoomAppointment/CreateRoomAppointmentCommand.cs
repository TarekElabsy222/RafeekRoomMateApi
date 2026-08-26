namespace RafeekRoomMate.Application.Features.RoomAppointments.Commands.CreateRoomAppointment
{
    public sealed class CreateRoomAppointmentCommand
    {

        public DateTime RequestedDate { get; init; }

        public TimeSpan RequestedStartTime { get; init; }

        public TimeSpan RequestedEndTime { get; init; }

        public string? Message { get; init; }
    }
}
