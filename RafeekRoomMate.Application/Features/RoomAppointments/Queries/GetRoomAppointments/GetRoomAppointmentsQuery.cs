namespace RafeekRoomMate.Application.Features.RoomAppointments.Queries.GetRoomAppointments
{
    public sealed class GetRoomAppointmentsQuery
    {
        public Guid RoomId { get; init; }

        public int PageNumber { get; init; } = 1;

        public int PageSize { get; init; } = 10;
    }
}
