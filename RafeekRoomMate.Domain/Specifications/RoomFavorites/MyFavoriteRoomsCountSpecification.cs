using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.RoomFavorites
{
    public sealed class MyFavoriteRoomsCountSpecification : BaseSpecification<Room>
    {
        public MyFavoriteRoomsCountSpecification(Guid userId)
            : base(x => x.Favorites.Any(f => f.UserId == userId))
        {
        }
    }
}
