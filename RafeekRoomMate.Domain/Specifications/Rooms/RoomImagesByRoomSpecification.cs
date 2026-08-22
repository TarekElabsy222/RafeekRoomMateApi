using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.Rooms
{
    public sealed class RoomImagesByRoomSpecification : BaseSpecification<RoomImage>
    {
        public RoomImagesByRoomSpecification(Guid roomId) : base(x => x.RoomId == roomId)
        {
        }
    }
}
