using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.RoomAppointments
{
    public sealed class RoomAppointmentByIdSpecification : BaseSpecification<RoomAppointment>
    {
        public RoomAppointmentByIdSpecification(Guid appointmentId)
            : base(x => x.Id == appointmentId)
        {
        }
    }
}
