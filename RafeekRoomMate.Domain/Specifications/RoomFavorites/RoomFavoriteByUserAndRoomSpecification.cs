using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.RoomFavorites
{
    public sealed class RoomFavoriteByUserAndRoomSpecification : BaseSpecification<RoomFavorite>
    {
        public RoomFavoriteByUserAndRoomSpecification( Guid userId, Guid roomId)
            : base(x => x.UserId == userId && x.RoomId == roomId)
        {
        }
    }
}
