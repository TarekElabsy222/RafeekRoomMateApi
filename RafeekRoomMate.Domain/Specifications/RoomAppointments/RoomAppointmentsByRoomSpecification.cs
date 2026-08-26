using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.RoomAppointments
{
    public sealed class RoomAppointmentsByRoomSpecification : BaseSpecification<RoomAppointment>
    {
        public RoomAppointmentsByRoomSpecification(
            Guid roomId, int skip, int take) : base(x => x.RoomId == roomId)
        {
            ApplyOrderByDescending(x => x.CreatedAt);

            ApplyPaging(skip, take);
        }
    }
}
