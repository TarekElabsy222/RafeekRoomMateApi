using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.RoomFavorites
{
    public sealed class MyFavoriteRoomsSpecification : BaseSpecification<Room>
    {
        public MyFavoriteRoomsSpecification( Guid userId, int skip, int take) : base(x => x.Favorites.Any(f => f.UserId == userId))
        {
            AddInclude(x => x.Images);

            ApplyOrderByDescending(x => x.CreatedAt);

            ApplyPaging(skip, take);
        }
    }
}
