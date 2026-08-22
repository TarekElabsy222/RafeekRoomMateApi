using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.RoomImages
{
    public sealed class RoomImageByIdAndRoomSpecification : BaseSpecification<RoomImage>
    {
        public RoomImageByIdAndRoomSpecification( Guid roomId, Guid imageId)
            : base(x => x.Id == imageId && x.RoomId == roomId)
        {
        }
    }
}
