using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.Rooms
{
    public sealed class RoomByIdSpecification : BaseSpecification<Room>
    {
        public RoomByIdSpecification(Guid roomId) : base(x => x.Id == roomId)
        {
        }
    }
}
