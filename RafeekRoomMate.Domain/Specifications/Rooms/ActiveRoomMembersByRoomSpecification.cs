using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.Rooms
{
    public sealed class ActiveRoomMembersByRoomSpecification : BaseSpecification<RoomMember>
    {
        public ActiveRoomMembersByRoomSpecification(Guid roomId)
            : base(x => x.RoomId == roomId && x.IsActive)
        {
        }
    }
}
