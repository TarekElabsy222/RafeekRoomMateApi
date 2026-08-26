using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Domain.Specifications.RoomAppointments
{
    public sealed class PendingRoomAppointmentSpecification : BaseSpecification<RoomAppointment>
    {
        public PendingRoomAppointmentSpecification(
            Guid roomId, Guid userId) : base(x => x.RoomId == roomId && x.UserId == userId && x.Status == RoomAppointmentStatus.Pending)
        {
        }
    }
}
