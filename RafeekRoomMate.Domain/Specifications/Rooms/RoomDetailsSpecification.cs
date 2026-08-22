using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.Rooms
{
    public sealed class RoomDetailsSpecification : BaseSpecification<Room>
    {
        public RoomDetailsSpecification(Guid roomId) : base(x => x.Id == roomId)
        {
            AddInclude(x => x.Images);

            AddInclude(x => x.Members);
        }
    }
}
